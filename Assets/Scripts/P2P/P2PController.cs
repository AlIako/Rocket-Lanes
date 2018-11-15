using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class P2PController : MonoBehaviour, INetworkController
{
	GameController gameController;
	string targetIp;

	void Start ()
	{
		gameController = GameObject.FindObjectOfType<GameController>();
	}

	public void Initialize()
	{

	}
	
    public int AskForConsent(ConsentAction consentAction, int[] parameters)
	{
		if(consentAction == ConsentAction.SpawnRocket)
		{
			//Vote necessary! Pick majority
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

	public bool HandleCollisions(Lane lane)
	{
		//only if its own lane
		return gameController.player.lane.id == lane.id;
	}

	public void NewGame()
	{
		Debug.Log("Starting New P2P Game...");
	}

	public void JoinGame()
	{
		GameObject IPField = GameObject.FindGameObjectWithTag("IPField");
		targetIp = IPField.GetComponent<InputField>().text;

		Debug.Log("Joining P2P Game " + targetIp + "...");
	}
}
