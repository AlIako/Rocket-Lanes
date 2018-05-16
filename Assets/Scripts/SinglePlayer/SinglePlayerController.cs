using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SinglePlayerController : MonoBehaviour, INetworkController
{
	[SerializeField]
	Player playerPrefab;

	[SerializeField]
	List<Transform> spawns;

	NetworkChoser.NetworkType netType;
	GameController gameController;

	void Start()
	{
		NetworkChoser networkChoser = FindObjectOfType<NetworkChoser>();
		netType = networkChoser.NetType;

		gameController = GameObject.FindObjectOfType<GameController>();

		Initialize();
	}
	
	public void Initialize()
	{
		//spawn player
		Player player1 = Instantiate(playerPrefab, spawns[0].transform.position, Quaternion.identity);
		player1.SetId(0);
		player1.PickColor();
		player1.ApplyColor();
		gameController.player = player1;
		

		//spawn AIs
		for(int i = 0; i < 3; i++)
		{
			Player playerAI = Instantiate(playerPrefab, spawns[1 + i].transform.position, Quaternion.identity);
			playerAI.SetId(1 + i);

			Destroy(playerAI.GetComponent<PlayerController>());
			Destroy(playerAI.GetComponent<NetworkTransform>());
			Destroy(playerAI.GetComponent<NetworkIdentity>());
			playerAI.gameObject.AddComponent<AI>();

			playerAI.PickColor();
			playerAI.ApplyColor();
		}
	}

    public void SpawnRocket(int fromPlayerId, int toPlayerId)
	{
		gameController.spawnerManagers[toPlayerId].Spawn();
	}
}
