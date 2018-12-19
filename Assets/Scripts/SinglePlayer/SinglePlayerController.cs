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
		//Debug.Log("Found gameController: " + gameController);

		Initialize();
	}
	
	public bool Initialize()
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
		return true;
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

    public void AskForConsent(ConsentMessage consentMessage)
	{
		ApplyConsent(consentMessage);
	}
	
    public void ApplyConsent(ConsentMessage consentMessage)
	{
		if(consentMessage.consentAction == ConsentAction.SpawnRocket)
		{
			bool cheating = !gameController.lanes[consentMessage.parameters[1]].spawnManager.ValidIndex(consentMessage.result);
			if(!cheating)
				gameController.lanes[consentMessage.parameters[1]].spawnManager.Spawn(consentMessage.result);
			else Debug.Log("Cheat!");
		}
		else if(consentMessage.consentAction == ConsentAction.CastShield)
		{
			Lane lane = gameController.lanes[consentMessage.parameters[0]];
			lane.player.CastShield();
		}
	}

	public bool HandleCollisions(Lane lane)
	{
		return true;
	}
}
