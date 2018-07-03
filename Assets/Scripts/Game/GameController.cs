using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public SpawnerManager[] spawnerManagers;

	[HideInInspector]
	public Player player;

	INetworkController networkController;

	void Start()
	{
		NetworkChoser networkChoser = GameObject.FindObjectOfType<NetworkChoser>();
		networkController = networkChoser.networkController;
	}

	public int ComputeNeighbourId(int playerId)
	{
		int neighbourPlayerId = -1;
		if(playerId != -1)
		{
			neighbourPlayerId = playerId + 1;
			if(neighbourPlayerId >= 4)
				neighbourPlayerId = 0;
		}
		Debug.Log("[GameController:ComputeNeighbourId] " + neighbourPlayerId);
		return neighbourPlayerId;
	}

	public void SendRocket() { SendRocket(player.Id, player.neighbourPlayerId); }
	public void SendRocket(int playerId, int neighbourPlayerId)
	{
		List<int> parameters = new List<int>();
		parameters.Add(playerId);
		parameters.Add(neighbourPlayerId);
		int[] parametersInt = parameters.ToArray();
		int consentResult = networkController.AskForConsent(ConsentAction.SpawnRocket, parametersInt);
		Debug.Log("Asking for consent " + ConsentAction.SpawnRocket + ", result: " + consentResult);
		if(consentResult != -1)
		{
			networkController.ApplyConsent(ConsentAction.SpawnRocket, parametersInt, consentResult);
		}
	}
}
