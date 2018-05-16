using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkManager
{
	int nextPlayerId = 0;
	NetworkStartPosition[] playerSpawns;

	void Start()
	{
		FindPlayerSpawns();
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject player = (GameObject)Instantiate(playerPrefab, playerSpawns[nextPlayerId].transform.position, Quaternion.identity);
        //player.GetComponent<Player>().color = Color.red;
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

		nextPlayerId ++;
		if(nextPlayerId >= 4)
			nextPlayerId = 0;
    }

	void FindPlayerSpawns()
	{
		playerSpawns = GameObject.FindObjectsOfType<NetworkStartPosition>();
	}
}
