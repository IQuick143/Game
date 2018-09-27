using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using NetworkMessageTypes;

/*
 * Class responsible for the networked backend of the Clients game
 */
public class GameClient : MonoBehaviour {
	
	public NetworkClient client = null;
	public bool connected {
		get {
			if (client == null) return false;
			return client.isConnected;
		}
	}
	public bool active = false;
	public bool isHost = false;

	private new string name = "";
	private string hostAddress;

	// Use this for initialization
	void Start() {
		InitializeClient();
	}
	
	// Update is called once per frame
	void Update() {
		if (connected) {
			var mes = new NetworkMessageTypes.ClientToServer.TestMessage();
			mes.data = "Foo";
			client.Send((short) NetworkMessageTypes.ClientToServer.ID.TestMessage, mes);
		}
	}

	public void SetData(string name, string address) {
		this.name = name;
		this.hostAddress = address;
	}

	public void InitializeClient() {
		if (client != null) {
			Debug.LogError("Already connected");
			return;
		}

		client = new NetworkClient();

		RegisterHandlers();

		//Parse address
		Uri url;
		int port = GameServer.port;
		if (Uri.TryCreate("http://" + hostAddress, UriKind.Absolute, out url)) {
			if (url.Port != 80) port = url.Port; 
			Debug.Log("IP: "+url.Host+" Port: "+port);
		} else {
			Debug.LogError("Error parsing IP Address");
			client.Shutdown();
			client = null;
			return;
		}

		client.Connect(url.Host, port);

		DontDestroyOnLoad(this.gameObject);
	}

	private void RegisterHandlers() {
		//System stuff
        client.RegisterHandler(MsgType.Connect, OnConnected);
        client.RegisterHandler(MsgType.Disconnect, OnDisconnected);
        client.RegisterHandler(MsgType.Error, OnError);
		//Connection stuff
		client.RegisterHandler((short) ServerToClient.ID.ClientRegistrationAcceptance, OnAcceptRegister);
		//Chat Stuff

		//Game stuff

	}

	private void OnConnected(NetworkMessage nmsg) {
		Debug.Log("Connected");
		Debug.Log("Sending Registration");
		var msg = new ClientToServer.RegisterClientMessage();
		msg.Name = this.name;
		client.Send((short) ClientToServer.ID.RegisterClient, msg);
	}

	private void OnDisconnected(NetworkMessage nmsg) {
		Debug.Log("Disconnected");
		ResetClient();
	}

	private void OnError(NetworkMessage nmsg) {
		Debug.Log("Error: "+nmsg.ToString());
	}

	private void OnAcceptRegister(NetworkMessage nmsg) {
		var msg = nmsg.ReadMessage<ServerToClient.ClientRegistrationAcceptanceMessage>();
		if (msg.Accepted) {
			Debug.Log("Accepted");
			this.active = true;
			this.isHost = msg.AcceptedAsHost;
		} else {
			Debug.LogWarning("Not Accepted");
			this.active = false;
			this.isHost = false;
			ResetClient();
		}
	}

	private void ResetClient() {
		//TODO: Idk something I guess
	}
}
