using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkManager, INetworkController
{
	int nextPlayerId = 0;
	NetworkStartPosition[] playerSpawns;

	void Start()
	{
		Initialize();
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

	public void Initialize()
	{
		playerSpawns = GameObject.FindObjectsOfType<NetworkStartPosition>();
	}
	
    /*public void SpawnRocket(int fromPlayerId, int toPlayerId)
	{
		NetworkServer.Spawn(spawnerManager.Spawn());
	}*/

    public int AskForConsent(ConsentAction consentAction, int[] parameters)
	{
		return 1;
	}
}
