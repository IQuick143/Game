using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class NetworkSetup : MonoBehaviour {

	private GameServer server;
	private GameClient client;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetupClient(string adress) {

	}

	public void SetupHost() {
		GameObject ServerObject = new GameObject("Server", new Type[]{typeof(GameServer), typeof(NetworkServerSimple)});
	}
}
