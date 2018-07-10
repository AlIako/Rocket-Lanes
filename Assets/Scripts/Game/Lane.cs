using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
	public int laneId = 0;
	public Player player;

	public bool IsOccupied()
	{
		return player != null;
	}

	public void Enter(Player player)
	{
		this.player = player;
	}

	public void Leave(Player player)
	{
		player = null;
	}

}
