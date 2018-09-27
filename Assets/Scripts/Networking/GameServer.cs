using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using NetworkMessageTypes;

/*
 * Class responsible for handling the gamestate and server communication
 */
public class GameServer : MonoBehaviour {

	public static readonly int port = 1337;
	private bool passwordProtected = false;
	private string Password = null;
	private bool hasHost = false;

	void Awake() {
		Initialize();
	}
	
	void Update() {
		
	}

	void MessageRecieved(NetworkMessage nmsg) {
		Debug.Log("Oshit");
	}

	public void Initialize() {
		if (NetworkServer.active) {
			Debug.LogWarning("Attemped initialize on active server.");
			return;
		}

		hasHost = false;

		NetworkServer.Listen(port);

		RegisterHandlers();

		DontDestroyOnLoad(this.gameObject);
	}
	
	public void ResetServer() {
		NetworkServer.Shutdown();
	}

	private void RegisterHandlers() {
		//Connection
		NetworkServer.RegisterHandler((short) ClientToServer.ID.RegisterClient, OnRegisterClient);
		//Chat
		//Debug
		NetworkServer.RegisterHandler((short) ClientToServer.ID.TestMessage, OnTestMessage);
	}

	//Handlers
	
	private void OnTestMessage(NetworkMessage nmsg) {
		Debug.Log(nmsg.ReadMessage<NetworkMessageTypes.ClientToServer.TestMessage>().data);
	}

	
	private void OnRegisterClient(NetworkMessage nmsg) {
		var msg = nmsg.ReadMessage<ClientToServer.RegisterClientMessage>();

		bool accept = true;
		if (passwordProtected) {
			if (msg.Password != Password) {
				accept = false;
			}
		}

		var message = new ServerToClient.ClientRegistrationAcceptanceMessage();
		message.Accepted = accept;
		if (accept && !hasHost) {
			message.AcceptedAsHost = true;
		}
		NetworkServer.SendToClient(nmsg.conn.connectionId, (short) ServerToClient.ID.ClientRegistrationAcceptance, message);

		//Do something to register the client
	}
}
