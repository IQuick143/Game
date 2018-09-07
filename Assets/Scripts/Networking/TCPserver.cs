using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;
using System;

public class TCPserver : MonoBehaviour {

	private TcpListener listener;
	private List<Client> clients;

	private Queue<OutgoingMessage> OutgoingMessages;

	private bool active = false;
	private bool accepting = false;

	private event EventHandler MessageRecieved;

	void Start () {
		
	}

	public void SendMessageAll(string message) {

	}

	public void SendMessage(Client client, string message) {
		OutgoingMessages.Enqueue(new OutgoingMessage(client, message));
	}

	private void IO() {
		while (active) {
			lock (clients) {
				foreach (Client client in clients) {
					string message = client.sr.ReadLine();
					if (message != null) {
						OnMessageRecieved(new ClientMessageRecievedEventArgs(client, ClientMessage.UnParseMessage(message)));
					}
				}
				if (OutgoingMessages.Count > 0) {
					foreach (Client client in clients) {
						if (OutgoingMessages.Peek().client.ID == client.ID) {

						}
					}
				}
			}
			Thread.Sleep(50);
		}
	}

	private void AcceptConnections() {
		while (accepting) {
			lock (listener) {
				
			}
			Thread.Sleep(250);
		}
	}

	protected virtual void OnMessageRecieved(ClientMessageRecievedEventArgs e) {
        if (MessageRecieved != null) {
			MessageRecieved(this, e);
		}
	}

	void OnDisable() {
		accepting = false;
		active = false;
	}
}

public class ClientMessageRecievedEventArgs : EventArgs {
	public string Message {get; set;}
	public DateTime Time {get; set;}

	public ClientMessageRecievedEventArgs(Client client, string message) {
		this.Message = message;
		this.Time = DateTime.Now;
	}
}

public class Client : IDisposable{
	private static int _id = 0;
	private static int nextID {
		get {
			return _id++;
		}
	}
	public TcpClient tcp;
	public NetworkStream stream;
	public StreamReader sr;
	public StreamWriter sw;
	public int ID;
	
	public Client(TcpClient TCP) {
		this.ID = nextID;
		this.tcp = TCP;
		this.stream = TCP.GetStream();
		this.sr = new StreamReader(this.stream);
		this.sw = new StreamWriter(this.stream);
	}

	public void Dispose() {
		this.sr.Close();
		this.sw.Close();
		if (stream != null) this.stream.Close();
	}
}

public struct OutgoingMessage {
	public ClientMessage message;
	public Client client;

	public OutgoingMessage(Client client, string message) {
		this.client = client;
		this.message = new ClientMessage(message);
	}

	public OutgoingMessage(Client client, string message, bool parsed) {
		this.client = client;
		this.message = new ClientMessage(message, parsed);
	}
}