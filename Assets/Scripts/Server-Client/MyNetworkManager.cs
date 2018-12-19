using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkManager, INetworkController
{
	GameController gameController;
	NetworkClient networkClient;

	public string targetIp = "localhost";

	public int playersMax = 4;
	float connectionRequestTimeoutTimeS = 2;
	float connectionRequestSentTime = 0;

	void Start()
	{
		gameController = GameObject.FindObjectOfType<GameController>();
	}

	public void Host()
	{
		StartHost();
		if(GameObject.FindGameObjectWithTag("SCUI") != null)
			GameObject.FindGameObjectWithTag("SCUI").SetActive(false);
	}

	public void Join()
	{
		networkAddress = targetIp;
		connectionRequestSentTime = Time.time;
		StartClient();
		if(GameObject.FindGameObjectWithTag("SCUI") != null)
			GameObject.FindGameObjectWithTag("SCUI").SetActive(false);
	}

	public void DisplayError(string er)
	{
		gameController.LeaveGame(false);
		FindObjectOfType<UIController>().DisplayError(er);
	}
	
	void Update()
	{
		if(connectionRequestSentTime != 0 && !GameController.gameStarted && GameObject.FindGameObjectWithTag("ErrorPanel") == null)
		{
			float currentTime = Time.time;
			if(currentTime - connectionRequestSentTime > connectionRequestTimeoutTimeS)
			{
				Debug.Log("Connection request timeout");
				DisplayError("Connection request timeout");
			}
		}
	}

	//Called on the server when a client adds a new player with ClientScene.AddPlayer.
	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
		int playersCount = GameObject.FindObjectsOfType<Player>().Length;
		if(playersCount >= playersMax)
		{
			conn.Disconnect();
			return;
		}

		Lane lane = gameController.GetFirstUnoccupiedLane();
        GameObject playerGameObject = (GameObject)Instantiate(playerPrefab, lane.startPosition.transform.position, Quaternion.identity);

		PlayerNetwork playerNetwork = playerGameObject.GetComponent<PlayerNetwork>();
		if(playerNetwork != null)
		{
			playerNetwork.enabled = true;
		}
		else Debug.Log("[MyNetworkManager:OnServerAddPlayer] playerNetwork script not found");

        NetworkServer.AddPlayerForConnection(conn, playerGameObject, playerControllerId);
    }

	public override void OnClientConnect(NetworkConnection connection)
    {
		//connection.
        Debug.Log(connection.connectionId + " Connected!");

		//Bypass "Client Ready" button
		ClientScene.Ready(connection);
		ClientScene.AddPlayer((short)connection.connectionId);

		//Reveal in-game UI
		gameController.StartGame();
    }

	public override void OnClientDisconnect(NetworkConnection conn)
	{
		DisplayError("Kicked from the game");
	}


	public bool Initialize()
	{
		NetworkServer.RegisterHandler(NetworkMessages.AskForConsentMsg, OnAskForConsentMsg);
		return true;
	}

	public override void OnStartHost()
	{
		Initialize();
		base.OnStartHost();
	}



	public void Quit()
	{
		StopClient();
		StopHost();
		StopServer();
		
		/*NetworkServer.UnregisterHandler(NetworkMessages.AskForConsentMsg);
		
		ClientScene.DestroyAllClientObjects();
		NetworkServer.SetAllClientsNotReady();
		NetworkServer.Reset();
		NetworkServer.Shutdown();*/
	}

    public void AskForConsent(ConsentMessage consentMessage)
	{
		networkClient = NetworkClient.allClients[0];
		networkClient.Send(NetworkMessages.AskForConsentMsg, consentMessage);

		if(Recorder.session != null)
		{
			Recorder.session.importantMessagesSent ++;
			Recorder.session.messagesSent ++;
		}
	}

	void OnAskForConsentMsg(NetworkMessage netMsg)
    {
        ConsentMessage msg = netMsg.ReadMessage<ConsentMessage>();
		if(msg.consentAction == ConsentAction.SpawnRocket)
			gameController.lanes[msg.parameters[1]].spawnManager.GetRandomSpawnerIndex();
		ApplyConsent(msg);
        Debug.Log("Received OnAskForConsentMsg " + msg.consentAction);
		
		if(Recorder.session != null)
		{
			Recorder.session.messagesReceived ++;
			Recorder.session.importantMessagesReceived ++;
		}
    }
	
    public void ApplyConsent(ConsentMessage consentMessage)
	{
		Debug.Log("Applying for consent " + consentMessage.consentAction);
		if(consentMessage.consentAction == ConsentAction.SpawnRocket)
		{
			if(NetworkServer.active)
			{
				bool cheating = !gameController.lanes[consentMessage.parameters[1]].spawnManager.ValidIndex(consentMessage.result);
				if(!cheating)
					NetworkServer.Spawn(gameController.lanes[consentMessage.parameters[1]].spawnManager.Spawn(consentMessage.result));
				else Debug.Log("Cheat!");
			}
		}
		else if(consentMessage.consentAction == ConsentAction.CastShield)
		{
			if(NetworkServer.active)
			{
				Lane lane = gameController.lanes[consentMessage.parameters[0]];
				lane.player.CastShield();
			}
		}
	}

	public bool HandleCollisions(Lane lane)
	{
		//only if server
		return NetworkServer.active;
	}
}
