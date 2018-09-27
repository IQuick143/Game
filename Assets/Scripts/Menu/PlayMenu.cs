using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private string adress = "";
	//private string name = "";
	void OnGUI() {
		adress = GUI.TextArea(new Rect(210, 70, 100, 50), adress);
		name = GUI.TextArea(new Rect(310, 70, 100, 100), name);
		if (GUI.Button(new Rect(10, 10, 200, 50), "Host a game")) {
			Host(name);
		}
		if (GUI.Button(new Rect(10, 70, 200, 50), "Join a game")) {
			Join(adress, name);
		}
	}

	void Join(string adress, string name) {
		GameObject g = new GameObject("Client", new System.Type[] {typeof(GameClient)});
		g.GetComponent<GameClient>().SetData(name, adress);
		SceneManager.LoadScene("ClientJoin");
	}

	void Host(string name) {
		SceneManager.LoadScene("HostHost");
	}
}
