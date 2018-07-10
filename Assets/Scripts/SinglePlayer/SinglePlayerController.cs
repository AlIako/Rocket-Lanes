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
		player1.gameObject.GetComponent<PlayerController>().enabled = true;
		gameController.player = player1;
		

		//spawn AIs
		for(int i = 0; i < 3; i++)
		{
			Player playerAI = Instantiate(playerPrefab, spawns[1 + i].transform.position, Quaternion.identity);
			playerAI.gameObject.AddComponent<AI>();
		}
	}

    public void SpawnRocket(int fromPlayerId, int toPlayerId)
	{
		int randomIndex = gameController.lanes[toPlayerId].spawnManager.GetRandomSpawnerIndex();
		gameController.lanes[toPlayerId].spawnManager.Spawn(randomIndex);
	}

    public int AskForConsent(ConsentAction consentAction, int[] parameters)
	{
		if(consentAction == ConsentAction.SpawnRocket)
		{
			//get random lane index from targeted spawnManager
			return gameController.lanes[parameters[1]].spawnManager.GetRandomSpawnerIndex();
		}
		return -1;
	}
	
    public void ApplyConsent(ConsentAction consentAction, int[] parameters, int consentResult)
	{
		if(consentAction == ConsentAction.SpawnRocket)
		{
			gameController.lanes[parameters[1]].spawnManager.Spawn(consentResult);
		}
	}
}
