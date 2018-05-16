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
        GameObject playerGameObject = (GameObject)Instantiate(playerPrefab, playerSpawns[nextPlayerId].transform.position, Quaternion.identity);
		Player player = playerGameObject.GetComponent<Player>();
		player.SetId(nextPlayerId);

        NetworkServer.AddPlayerForConnection(conn, playerGameObject, playerControllerId);

		nextPlayerId ++;
		if(nextPlayerId >= 4)
			nextPlayerId = 0;
    }

	void FindPlayerSpawns()
	{
		playerSpawns = GameObject.FindObjectsOfType<NetworkStartPosition>();
	}
}
