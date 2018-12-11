using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	public Lane[] lanes;

	[SerializeField]
	GameObject networkControllerGameObject;

	public CheaterProfileContainer cheaterProfileContainer;

	[HideInInspector]
	public Player player;
	
	[HideInInspector]
	public int playersCount = 0; //small delay
	private int UpdatePlayersCountDelay = 1;

	public static bool gameStarted = false;

	[HideInInspector]
	public Recorder recorder;

	INetworkController networkController;

	void Start()
	{
		networkController = networkControllerGameObject.GetComponent<INetworkController>();
	}

	public void StartGame()
	{
		GameObject.FindObjectOfType<UIController>().EnterGameUI();
		gameStarted = true;

		recorder = GameObject.FindObjectOfType<Recorder>();
		recorder.StartRecording();
	}

	public void LeaveGame(bool enterUI = true)
	{
		gameStarted = false;
		if(recorder != null)
			recorder.StopRecording();
		networkController.Quit();
		
		player = null;

		if(enterUI)
        	SceneManager.LoadScene("Main Menu");
	}

	public IEnumerator UpdatePlayersCount()
	{
		yield return new WaitForSeconds(UpdatePlayersCountDelay);
		playersCount = 0;
		foreach(Lane lane in lanes)
		{
			if(lane.IsOccupied())
				playersCount ++;
		}
		
		if(recorder != null)
			recorder.UpdatePlayersCount(playersCount);
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

	public int GetNextAliveLaneId(Player p)
	{
		int laneId = p.lane.id;
		for(int i = 0; i < 3; i ++)
		{
			laneId ++;
			if(laneId >= 4)
				laneId = 0;
			
			if(lanes[laneId].PlayerAlive())
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

	public void SendRocket() { SendRocket(player.lane.id, GetNextAliveLaneId(player)); }
	public void SendRocket(int playerId, int neighbourPlayerId)
	{
		if(lanes[playerId].player.Health <= 0) //dead players cant send rockets
			return;
		
		ConsentMessage consentMessage = new ConsentMessage();
		consentMessage.consentAction = ConsentAction.SpawnRocket;
		consentMessage.parameters.Add(playerId);
		consentMessage.parameters.Add(neighbourPlayerId);
		consentMessage.result = lanes[neighbourPlayerId].spawnManager.GetRandomSpawnerIndex();

		//Cheating only implemented in P2PController
		/*//will I cheat?
		int randomInt = Random.Range(0, 100);
		if(randomInt < cheaterProfileContainer.cheaterProfile.cheatingRate)
		{
			if(Recorder.session != null)
				Recorder.session.cheatsTried ++;

			//sending a wrong spawner index
			consentMessage.result = 100;
		}*/
		
		networkController.AskForConsent(consentMessage);
	}

	public bool HandleCollisions(Lane lane)
	{
		if(lane == null)
			return false;
		bool result = networkController.HandleCollisions(lane);
		//Debug.Log("Handle Collisions for lane " + lane.id + ": " + result);
		return result;
	}
}
