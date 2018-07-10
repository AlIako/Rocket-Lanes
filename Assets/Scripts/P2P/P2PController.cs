using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2PController : MonoBehaviour, INetworkController
{
	GameController gameController;
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
}
