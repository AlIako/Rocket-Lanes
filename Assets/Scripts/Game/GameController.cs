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

	public int ComputeNeighbourId(Player player)
	{
		//find the nearest player right of you. if none, find the most left player
		double playerX = player.transform.position.x;
		Player[] players = GameObject.FindObjectsOfType<Player>();
		Player nearestRight = null;
		Player mostLeft = null;
		foreach(Player p in players)
		{
			//dont take self into account
			if(p != player)
			{
				if(p.transform.position.x > playerX)
				{
					if(nearestRight == null || p.transform.position.x < nearestRight.transform.position.x)
						nearestRight = p;
				}
				else if(p.transform.position.x < playerX)
				{
					if(mostLeft == null || p.transform.position.x < mostLeft.transform.position.x)
						mostLeft = p;
				}
			}
		}

		int neighbourPlayerId = -1;
		if(nearestRight != null)
		{
			neighbourPlayerId = nearestRight.Id;
		}
		else if(mostLeft != null)
		{
			neighbourPlayerId = mostLeft.Id;
		}
		else if(player.Id != -1)
		{
			neighbourPlayerId = player.Id + 1;
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
