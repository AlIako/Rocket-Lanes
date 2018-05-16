using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UIGame : MonoBehaviour
{
	SpawnerManager spawnerManager;
	NetworkManager networkManager;

	void Start()
	{
		networkManager = GameObject.FindObjectOfType<NetworkManager>();
	}
	
	public void SendRocket()
	{
		FindSpawnerManager();
		NetworkServer.Spawn(spawnerManager.Spawn());
	}

	void FindSpawnerManager()
	{
		SpawnerManager[] spawnerManagers = GameObject.FindObjectsOfType<SpawnerManager>();
		Player[] players = GameObject.FindObjectsOfType<Player>();
		int playerId = -1;
		foreach(Player player in players)
		{
			if(player.GetComponent<NetworkIdentity>().isLocalPlayer)
				playerId = player.Id;
		}

		int neighbourPlayerId = -1;
		if(playerId != -1)
		{
			neighbourPlayerId = playerId + 1;
			if(neighbourPlayerId >= 4)
				neighbourPlayerId = 0;
		}

		spawnerManager = spawnerManagers[neighbourPlayerId];
	}
}
