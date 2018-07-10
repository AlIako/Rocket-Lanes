using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public SpawnerManager[] spawnerManagers;
	public Lane[] lanes;

	[HideInInspector]
	public Player player;

	INetworkController networkController;

	void Start()
	{
		NetworkChoser networkChoser = GameObject.FindObjectOfType<NetworkChoser>();
		networkController = networkChoser.networkController;
	}

	public int GetNextOccupiedLaneId(Player player)
	{
		int laneId = player.Id;
		for(int i = 0; i < 4; i ++)
		{
			laneId ++;
			if(laneId >= 4)
				laneId = 0;
			
			if(lanes[laneId].IsOccupied())
				return laneId;
		}
		return -1;
	}

	public void SendRocket() { SendRocket(player.Id, GetNextOccupiedLaneId(player)); }
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
