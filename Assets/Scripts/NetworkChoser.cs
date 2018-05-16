using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkChoser : MonoBehaviour
{
	[SerializeField]
	GameObject inGameUI;
	
	[SerializeField]
	GameObject lobbyUI;
	
	[SerializeField]
	GameObject serverClientNetworkManager;

	public enum NetworkType {Singleplayer, ServerClient, P2P};
	[SerializeField]
	private NetworkType networkType = NetworkType.Singleplayer;
	public NetworkType GetNetworkType { get { return networkType; } }

	public void PickNetworkType(int networkType)
	{
		//can't pass enum as parameter of OnClick callback
		if(networkType == 0)
			this.networkType = NetworkType.Singleplayer;
		else if(networkType == 1)
			this.networkType = NetworkType.ServerClient;
		else if(networkType == 2)
			this.networkType = NetworkType.P2P;

		lobbyUI.SetActive(false);
		inGameUI.SetActive(true);

		if(this.networkType == NetworkType.ServerClient)
			serverClientNetworkManager.SetActive(true);
	}
}
