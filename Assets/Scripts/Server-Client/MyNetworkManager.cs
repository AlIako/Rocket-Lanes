using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkManager, INetworkController
{
	GameController gameController;
	NetworkClient networkClient;

	void Start()
	{
		gameController = GameObject.FindObjectOfType<GameController>();
	}

	//Called on the server when a client adds a new player with ClientScene.AddPlayer.
	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
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
        Debug.Log(connection.connectionId + " Connected!");

		//Bypass "Client Ready" button
		ClientScene.Ready(connection);
		ClientScene.AddPlayer((short)connection.connectionId);

		//Reveal in-game UI
		gameController.StartGame();
    }

	public void Initialize()
	{
		NetworkServer.RegisterHandler(NetworkMessages.AskForConsentMsg, OnAskForConsentMsg);
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

    public int AskForConsent(ConsentAction consentAction, int[] parameters)
	{
		IntArrayMessage msg = new IntArrayMessage();
		msg.consentAction = consentAction;
		msg.parameters = parameters;

		networkClient = NetworkClient.allClients[0];
		networkClient.Send(NetworkMessages.AskForConsentMsg, msg);
		return -1;
	}

	void OnAskForConsentMsg(NetworkMessage netMsg)
    {
		int result = -1;
        var msg = netMsg.ReadMessage<IntArrayMessage>();
		if(msg.consentAction == ConsentAction.SpawnRocket)
		{
			result = gameController.lanes[msg.parameters[1]].spawnManager.GetRandomSpawnerIndex();
		}
		ApplyConsent(msg.consentAction, msg.parameters, result);
        Debug.Log("Received OnAskForConsentMsg " + msg.consentAction);
    }
	
    public void ApplyConsent(ConsentAction consentAction, int[] parameters, int consentResult)
	{
		Debug.Log("Applying for consent " + consentAction);
		if(consentAction == ConsentAction.SpawnRocket)
		{
			if(NetworkServer.active)
			{
				NetworkServer.Spawn(gameController.lanes[parameters[1]].spawnManager.Spawn(consentResult));
			}
		}
	}

	public bool HandleCollisions(Lane lane)
	{
		//only if server
		return NetworkServer.active;
	}
}
