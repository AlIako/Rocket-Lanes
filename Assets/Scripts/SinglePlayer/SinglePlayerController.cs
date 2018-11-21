using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SinglePlayerController : MonoBehaviour, INetworkController
{
	[SerializeField]
	Player playerPrefab;

	GameController gameController;

	void Start()
	{
		gameController = GameObject.FindObjectOfType<GameController>();
		Debug.Log("Found gameController: " + gameController);

		Initialize();
	}
	
	public void Initialize()
	{
		//spawn player
		Player player1 = Instantiate(playerPrefab, gameController.lanes[0].startPosition.transform.position, Quaternion.identity);
		player1.gameObject.GetComponent<PlayerController>().enabled = true;
		gameController.player = player1;
		

		//spawn AIs
		for(int i = 0; i < 3; i++)
		{
			Player playerAI = Instantiate(playerPrefab, gameController.lanes[1 + i].startPosition.transform.position, Quaternion.identity);
			playerAI.gameObject.AddComponent<AI>();
		}

		gameController.StartGame();
	}

	public void Quit()
	{
		//destroy rockets and players
		Rocket[] rockets = FindObjectsOfType<Rocket>();
		foreach(Rocket rocket in rockets)
			Destroy(rocket.gameObject);
			
		Player[] players = FindObjectsOfType<Player>();
		foreach(Player p in players)
			Destroy(p.gameObject);
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
			//gameController = GameObject.FindObjectOfType<GameController>();
			return gameController.lanes[parameters[1]].spawnManager.GetRandomSpawnerIndex();
		}
		return -1;
	}
	
    public void ApplyConsent(ConsentAction consentAction, int[] parameters, int consentResult)
	{
		if(consentAction == ConsentAction.SpawnRocket)
		{
			Debug.Log("ap1: " + parameters[1] + ", lane count: " + gameController.lanes.Length);
			gameController.lanes[parameters[1]].spawnManager.Spawn(consentResult);
		}
	}

	public bool HandleCollisions(Lane lane)
	{
		return true;
	}
}
