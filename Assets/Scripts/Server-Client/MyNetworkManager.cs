using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkManager, INetworkController
{
	int nextPlayerId = 0;
	NetworkStartPosition[] playerSpawns;
	GameController gameController;

	void Start()
	{
		gameController = GameObject.FindObjectOfType<GameController>();
		Initialize();
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject playerGameObject = (GameObject)Instantiate(playerPrefab, playerSpawns[nextPlayerId].transform.position, Quaternion.identity);
		Player player = playerGameObject.GetComponent<Player>();
		player.SetId(nextPlayerId);

        NetworkServer.AddPlayerForConnection(conn, playerGameObject, playerControllerId);

		nextPlayerId ++;
		if(nextPlayerId >= 4)
			nextPlayerId = 0;
    }

	public void Initialize()
	{
		playerSpawns = GameObject.FindObjectsOfType<NetworkStartPosition>();
	}

    public int AskForConsent(ConsentAction consentAction, int[] parameters)
	{
		return gameController.spawnerManagers[parameters[1]].GetRandomSpawnerIndex();
	}
	
    public void ApplyConsent(ConsentAction consentAction, int[] parameters, int consentResult)
	{
		if(consentAction == ConsentAction.SpawnRocket)
		{
			if(Network.isServer)
				NetworkServer.Spawn(gameController.spawnerManagers[parameters[1]].Spawn(consentResult));
		}
	}
}
