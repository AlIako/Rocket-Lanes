using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkManager, INetworkController
{
	int nextPlayerId = 0;
	public NetworkStartPosition[] playerSpawns;
	GameController gameController;
	NetworkClient networkClient;

	void Start()
	{
		gameController = GameObject.FindObjectOfType<GameController>();
		Initialize();
	}

	//Called on the server when a client adds a new player with ClientScene.AddPlayer.
	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject playerGameObject = (GameObject)Instantiate(playerPrefab, playerSpawns[nextPlayerId].transform.position, Quaternion.identity);
		Player player = playerGameObject.GetComponent<Player>();
		player.SetId(nextPlayerId);
		player.PickColor();

		PlayerNetwork playerNetwork = playerGameObject.GetComponent<PlayerNetwork>();
		if(playerNetwork != null)
		{
			playerNetwork.enabled = true;
			playerNetwork.SetColor(player.Color);
		}
		else Debug.Log("[MyNetworkManager:OnServerAddPlayer] playerNetwork script not found");

        NetworkServer.AddPlayerForConnection(conn, playerGameObject, playerControllerId);

		nextPlayerId ++;
		if(nextPlayerId >= 4)
			nextPlayerId = 0;
    }

	public void Initialize()
	{
		NetworkServer.RegisterHandler(NetworkMessages.AskForConsentMsg, OnAskForConsentMsg);
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
			result = gameController.spawnerManagers[msg.parameters[1]].GetRandomSpawnerIndex();
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
				NetworkServer.Spawn(gameController.spawnerManagers[parameters[1]].Spawn(consentResult));
			}
		}
	}
}
