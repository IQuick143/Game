using UnityEngine.Networking;

namespace NetworkMessageTypes {

	//Client to Server message ids
	public class ClientToServer {
		public enum ID : short {
			RegisterClient = 100,
			SendGameAction = 110,
			RequestChat = 120,
			SendChatMessage = 121,
			RequestGameState = 130,
			TestMessage = 155
		}

		public class RegisterClientMessage : MessageBase {
			public string Name;
			public string Password = "";
		}

		public class TestMessage : MessageBase {
			public string data;
		}
	}

	//Server to Client message ids
	public class ServerToClient {
		
		public enum ID : short {
			ClientRegistrationAcceptance = 200,
			RequestGameAction = 210,
			SendChatMessages = 220,
			NewChatMessage = 221,
			SendGameState = 230
		}

		public class ClientRegistrationAcceptanceMessage : MessageBase {
			public bool Accepted = false;
			public bool AcceptedAsHost = false;
		}
	}
}
