using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/*
 * Class responsible for handling the gamestate and server communication
 */
public class GameServer : MonoBehaviour {

	public static readonly int port = 1337;

	// Use this for initialization
	void Awake() {
		Initialize();
	}
	
	// Update is called once per frame
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

		NetworkServer.Listen(port);

		RegisterHandlers();

		DontDestroyOnLoad(this.gameObject);
	}
	
	public void ResetServer() {
		NetworkServer.Shutdown();
	}

	private void RegisterHandlers() {

	}
}
