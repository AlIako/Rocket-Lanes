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
	
	float lastTimeSecond = 0;

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

		CountMessagesPerEntity();
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

		lastTimeSecond = Time.time;
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

	public override void OnStartClient(NetworkClient client)
	{
		if(!IsServer())
		{
			networkClient = NetworkClient.allClients[0];
			networkClient.RegisterHandler(NetworkMessages.ApplyConsentMsg, OnApplyConsentMsg);
		}
		base.OnStartClient(client);
	}



	public void Quit()
	{
		StopClient();
		StopHost();
		StopServer();
	}

	void CountMessagesPerEntity()
	{
		/*
		Count "manually" how many messages are sent / received in ServerClient: 
		Each moment a rocket/player is alive, count messages sent / received depending 
		on tickrate and status (host or client). Use Time deltas for this
		 */
		if(Recorder.session != null)
		{
			float currentTime = Time.time;

			if(currentTime - lastTimeSecond > 1)
			{
				lastTimeSecond ++;
				if(NetworkServer.active) //server
				{
					/*
					3x out : player sync 9 per sec
					3x in : player sync 9 per sec
					*/
					int clientsCount = GameObject.FindObjectsOfType<Player>().Length - 1;
					Recorder.session.messagesSent += clientsCount * 9;
					Recorder.session.messagesReceived += clientsCount * 9;
				}
				else //client
				{
					/*
					1x out : player sync
					1x in : player sync
					*/
					Recorder.session.messagesSent += 9;
					Recorder.session.messagesReceived += 9;
				}
			}
		}
	}

	public bool IsServer()
	{
		return NetworkServer.active;
	}

    public void AskForConsent(ConsentMessage consentMessage)
	{
		networkClient = NetworkClient.allClients[0];
		networkClient.Send(NetworkMessages.AskForConsentMsg, consentMessage);

		if(Recorder.session != null)
		{
			Recorder.session.importantMessagesSent ++;
			Recorder.session.messagesSent ++;
			Recorder.session.consentSent ++;
		}
	}

	void OnAskForConsentMsg(NetworkMessage netMsg)
    {
        ConsentMessage msg = netMsg.ReadMessage<ConsentMessage>();
		if(msg.consentAction == ConsentAction.SpawnRocket)
			gameController.lanes[msg.parameters[1]].spawnManager.GetRandomSpawnerIndex();
		ApplyConsent(msg);

		//send apply consent confirmation
		NetworkServer.SendToAll(NetworkMessages.ApplyConsentMsg, msg);
		
		if(Recorder.session != null)
		{
			Recorder.session.messagesReceived ++;
			Recorder.session.importantMessagesReceived ++;

			//Send consent apply to each client
			int clientsCount = GameObject.FindObjectsOfType<Player>().Length - 1;
			Recorder.session.importantMessagesSent += clientsCount;
			Recorder.session.messagesSent += clientsCount;
		}
    }

	void OnApplyConsentMsg(NetworkMessage netMsg)
    {
        ConsentMessage msg = netMsg.ReadMessage<ConsentMessage>();
		if(Recorder.session != null)
		{
			Recorder.session.AddSentAndAppliedConsent(msg.timestampSendMs);

			//Receive consent apply
			Recorder.session.messagesReceived ++;
			Recorder.session.importantMessagesReceived ++;
		}
    }
	
    public void ApplyConsent(ConsentMessage consentMessage, bool wasMyRequest = false)
	{
		//Debug.Log("Applying for consent " + consentMessage.consentAction);
		if(consentMessage.consentAction == ConsentAction.SpawnRocket)
		{
			if(NetworkServer.active)
			{
				bool cheating = !gameController.lanes[consentMessage.parameters[1]].spawnManager.ValidIndex(consentMessage.result);
				if(!cheating)
					NetworkServer.Spawn(gameController.lanes[consentMessage.parameters[1]].spawnManager.Spawn(consentMessage.result));
				else Debug.Log("Cheat! wrong rocket infos");
			}
		}
		else if(consentMessage.consentAction == ConsentAction.CastShield)
		{
			if(NetworkServer.active)
			{
				Lane lane = gameController.lanes[consentMessage.parameters[0]];
				if(lane.player.ShieldReady())
					lane.player.CastShield();
				else Debug.Log("Cheat! Shield not ready");
			}
		}
	}

	public bool HandleCollisions(Lane lane)
	{
		//only if server
		return NetworkServer.active;
	}
}
