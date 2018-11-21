using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public Lane[] lanes;

	[SerializeField]
	GameObject InGameUI;

	[HideInInspector]
	public Player player;

	[HideInInspector]
	public INetworkController networkController;
	
	NetworkChoser networkChoser;

	public static bool gameStarted = false;

	void Start()
	{
		networkChoser = GameObject.FindObjectOfType<NetworkChoser>();
		networkController = networkChoser.networkController;
	}

	public void StartGame()
	{
		//Reveal in-game UI
		InGameUI.SetActive(true);
		gameStarted = true;
	}

	public void LeaveGame(bool enterUI = true)
	{
		networkController.Quit();
		
		InGameUI.SetActive(false);
		gameStarted = false;
		player = null;

		if(enterUI)
			networkChoser.EnterLobbyUI();
	}

	public int GetNextOccupiedLaneId(Player p)
	{
		int laneId = p.lane.id;
		for(int i = 0; i < 3; i ++)
		{
			laneId ++;
			if(laneId >= 4)
				laneId = 0;
			
			if(lanes[laneId].IsOccupied())
				return laneId;
		}
		
		//if no other player, just send to next lane
		laneId = p.lane.id;
		laneId ++;
		if(laneId >= 4)
			laneId = 0;
		
		return laneId;
	}

	public Lane GetFirstUnoccupiedLane()
	{
		foreach(Lane lane in lanes)
		{
			if(!lane.IsOccupied())
				return lane;
		}
		return null;
	}

	public void SendRocket() { SendRocket(player.lane.id, GetNextOccupiedLaneId(player)); }
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

	public bool HandleCollisions(Lane lane)
	{
		if(lane == null)
			return false;
		bool result = networkController.HandleCollisions(lane);
		Debug.Log("Handle Collisions for lane " + lane.id + ": " + result);
		return result;
	}
}
