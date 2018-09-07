using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;
using System;

public class TCPclient : MonoBehaviour {

	private bool connected = false;
	private TcpClient tcp;
	private Thread IOthread;
	private NetworkStream stream;
	private Queue<ClientMessage> messageQueue;

	public event EventHandler MessageRecieved;

	void Awake() {
		tcp = new TcpClient();
		messageQueue = new Queue<ClientMessage>();
	}

	void OnDisable() {
		if (connected) Disconnect();
	}

	public void Connect(int port, IPAddress address) {
		tcp.Connect(address, port);
		connected = true;
		SetupIO();
	}

	public void Disconnect() {
		connected = false;
		tcp.Close();
	}

	private void SetupIO() {
		stream = tcp.GetStream();
		IOthread = new Thread(new ThreadStart(IO));
		IOthread.Start();
	}

	private void IO() {
		StreamReader sr = new StreamReader(stream);
		StreamWriter sw = new StreamWriter(stream);
		while (connected) {
			lock (messageQueue) {
				if (messageQueue.Count > 0) {
					sw.Write(messageQueue.Dequeue().GetParsedMessage());
				}
			}
			string mes = sr.ReadLine();
			if (mes != null) {
				mes = ClientMessage.UnParseMessage(mes);
				if (mes.StartsWith("PING")) {
					sw.Write(ClientMessage.ParseMessage(mes.Replace("PING", "PONG")));
					Debug.Log("PING PONG");
				} else {
					OnMessageRecieved(new MessageRecievedEventArgs(mes));
				}
			}
			Thread.Sleep(50);
		}
		sr.Close();
		sw.Close();
	}

	protected virtual void OnMessageRecieved(MessageRecievedEventArgs e) {
        if (MessageRecieved != null) {
			MessageRecieved(this, e);
		}
	}
}

public class MessageRecievedEventArgs : EventArgs {
	public string Message {get; set;}
	public DateTime Time {get; set;}

	public MessageRecievedEventArgs(string message) {
		this.Message = message;
		this.Time = DateTime.Now;
	}
}

public class ClientMessage {
	public static readonly char delimiter = '\n';
	//Use this to replace the delimiter
	public static readonly string delimiterRep = @"\n";

	private string Message;
	private string ParsedMessage;

	public ClientMessage(string msg) {
		Message = msg;
		ParsedMessage = ParseMessage(msg);
	}

	public ClientMessage(string msg, bool parsed) {
		if (!parsed) {
			Message = msg;
			ParsedMessage = ParseMessage(msg);
			return;
		}
		Message = UnParseMessage(msg);
		ParsedMessage = msg;
	}

	public string GetMessage() {
		return Message;
	}
	public string GetParsedMessage() {
		return ParsedMessage;
	}

	public static string ParseMessage(string message) {
		return message.Replace(@"\", @"\ ").Replace(""+delimiter, delimiterRep)+delimiter;
	}

	public static string UnParseMessage(string parsedmessage) {
		return parsedmessage.Substring(0,parsedmessage.Length-1).Replace(delimiterRep, ""+delimiter).Replace(@"\ ", @"\");
	}
}
