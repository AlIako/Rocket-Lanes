using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Lane : MonoBehaviour
{
	public int id = 0;
	public Player player;
	public SpawnerManager spawnManager;
	public NetworkStartPosition startPosition;
	
	private UIController uIController;
	private GameController gameController;
	Color color = new Color(1.0f, 1.0f, 1.0f);
	private bool isRequested = false;
	private float timeRequest = 0;

	void Start()
	{
		PickColor();
	}

	public void Request()
	{
		isRequested = true;
		timeRequest = Time.time;
	}

	public bool IsRequested()
	{
		return isRequested;
	}

	void Update()
	{
		if(isRequested == true)
		{
			if(Time.time - timeRequest > 5)
			{
				isRequested = false;
				timeRequest = 0;
			}
		}
	}

	void PickColor()
	{
		if(id == 0)
			color = new Color(1.0f, 0.0f, 0.0f);
		else if(id == 1)
			color = new Color(0.0f, 0.3f, 1.0f);
		else if(id == 2)
			color = new Color(0.3f, 0.85f, 0.0f);
		else if(id == 3)
			color = new Color(1.0f, 1.0f, 0.0f);
	}

	public bool IsOccupied()
	{
		return player != null;
	}

	public bool PlayerAlive()
	{
		return player != null && player.Health > 0;
	}

	public void Enter(Player player)
	{
		this.player = player;
		player.lane = this;
		player.ApplyColor(this.color);
		
		if(uIController == null)
			uIController = FindObjectOfType<UIController>();
		uIController.ActivateLaneUI(id);

		if(gameController == null)
			gameController = FindObjectOfType<GameController>();
		StartCoroutine(gameController.UpdatePlayersCount());
	}

	public void Leave(Player player)
	{
		player.lane = null;
		player = null;
		
		if(uIController == null)
			uIController = FindObjectOfType<UIController>();
		uIController.DeactivateLaneUI(id);
		
		if(gameController == null)
			gameController = FindObjectOfType<GameController>();
		StartCoroutine(gameController.UpdatePlayersCount());
	}

}
