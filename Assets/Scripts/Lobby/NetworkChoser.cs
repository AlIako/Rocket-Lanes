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
	GameObject gameController;
	
	[SerializeField]
	GameObject singlePlayerController;
	
	[SerializeField]
	GameObject serverClientNetworkManager;
	
	[SerializeField]
	GameObject P2PController;

	public enum NetworkType {Singleplayer, ServerClient, P2P};
	[SerializeField]
	private NetworkType netType = NetworkType.Singleplayer;
	public NetworkType NetType { get { return netType; } }
	public INetworkController networkController;

	public void PickNetworkType(int type)
	{
		ApplyNetworkFromInt(type);
		EnterGameUI();
		ActivateControllers();
	}

	void ApplyNetworkFromInt(int type)
	{
		//can't pass enum as parameter of OnClick callback
		this.netType = (NetworkType)type;
		if(this.netType == NetworkType.Singleplayer)
			networkController = singlePlayerController.GetComponent<SinglePlayerController>();
		else if(this.netType == NetworkType.ServerClient)
			networkController = serverClientNetworkManager.GetComponent<MyNetworkManager>();
		else if(this.netType == NetworkType.P2P)
			networkController = P2PController.GetComponent<P2PController>();
	}

	void EnterGameUI()
	{
		lobbyUI.SetActive(false);
		inGameUI.SetActive(true);
	}
	void ActivateControllers()
	{
		if(this.netType == NetworkType.Singleplayer)
			singlePlayerController.SetActive(true);
		else if(this.netType == NetworkType.ServerClient)
			serverClientNetworkManager.SetActive(true);
		else if(this.netType == NetworkType.P2P)
			P2PController.SetActive(true);
		
		gameController.SetActive(true);
	}
}
