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

	GameController gameController;

	void Start()
	{
		gameController = GameObject.FindObjectOfType<GameController>();

		Initialize();
	}
	
	public void Initialize()
	{
		//spawn player
		Player player1 = Instantiate(playerPrefab, spawns[0].transform.position, Quaternion.identity);
		player1.SetId(0);
		player1.SetNeighbourId(gameController.ComputeNeighbourId(player1.Id));
		player1.PickColor();
		player1.ApplyColor();
		player1.gameObject.GetComponent<PlayerController>().enabled = true;
		gameController.player = player1;
		

		//spawn AIs
		for(int i = 0; i < 3; i++)
		{
			Player playerAI = Instantiate(playerPrefab, spawns[1 + i].transform.position, Quaternion.identity);
			playerAI.SetId(1 + i);
			playerAI.SetNeighbourId(gameController.ComputeNeighbourId(playerAI.Id));
			
			playerAI.gameObject.AddComponent<AI>();

			playerAI.PickColor();
			playerAI.ApplyColor();
		}
	}

    public void SpawnRocket(int fromPlayerId, int toPlayerId)
	{
		int randomIndex = gameController.spawnerManagers[toPlayerId].GetRandomSpawnerIndex();
		gameController.spawnerManagers[toPlayerId].Spawn(randomIndex);
	}

    public int AskForConsent(ConsentAction consentAction, int[] parameters)
	{
		if(consentAction == ConsentAction.SpawnRocket)
		{
			//get random lane index from targeted spawnManager
			return gameController.spawnerManagers[parameters[1]].GetRandomSpawnerIndex();
		}
		return -1;
	}
	
    public void ApplyConsent(ConsentAction consentAction, int[] parameters, int consentResult)
	{
		if(consentAction == ConsentAction.SpawnRocket)
		{
			gameController.spawnerManagers[parameters[1]].Spawn(consentResult);
		}
	}
}
