using UnityEngine.Networking;

namespace NetworkMessageTypes {

	//Client to Server message ids
	public class ClientToServer {
		public enum ID : short {
			RegisterClient = 100,
			SendGameAction = 110,
			RequestChat = 120,
			RequestGameState = 130
		}
	}

	//Server to Client message ids
	public class ServerToClient {
		
		public enum ID : short {
			StartGame = 200,
			RequestGameAction = 210,
			SendChatMessages = 220,
			NewChatMessage = 221,
			SendGameState = 230
		}
	}
}