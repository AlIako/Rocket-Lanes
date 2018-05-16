using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public SpawnerManager[] spawnerManagers;

	[HideInInspector]
	public Player player;

	INetworkController networkController;

	void Start ()
	{
		NetworkChoser networkChoser = GameObject.FindObjectOfType<NetworkChoser>();
		networkController = networkChoser.networkController;
	}

	public void SendRocket()
	{
		SendRocket(player.Id, player.neighbourPlayerId);
	}

	public void SendRocket(int playerId, int neighbourPlayerId)
	{
		networkController.SpawnRocket(playerId, neighbourPlayerId);
	}
}
