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
	Color color = new Color(1.0f, 1.0f, 1.0f);

	void Start()
	{
		PickColor();
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

	public void Enter(Player player)
	{
		this.player = player;
		player.lane = this;
		player.ApplyColor(this.color);
		
		if(uIController == null)
			uIController = FindObjectOfType<UIController>();
		uIController.ActivateLaneUI(id);
	}

	public void Leave(Player player)
	{
		player.lane = null;
		player = null;
		
		if(uIController == null)
			uIController = FindObjectOfType<UIController>();
		uIController.DeactivateLaneUI(id);
	}

}
