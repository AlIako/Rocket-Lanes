using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UIGame : MonoBehaviour
{
	[SerializeField]
	SpawnerManager spawnerManager;

	NetworkManager networkManager;

	void Start()
	{
		networkManager = GameObject.FindObjectOfType<NetworkManager>();
	}
	
	public void SendRocket()
	{
		NetworkServer.Spawn(spawnerManager.Spawn());
	}
}
