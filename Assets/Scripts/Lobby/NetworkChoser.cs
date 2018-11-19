using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkChoser : MonoBehaviour
{
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
	
	[SerializeField]
	GameObject ChoseP2P;

	public enum NetworkType {Singleplayer, ServerClient, P2P};
	[SerializeField]
	private NetworkType netType = NetworkType.Singleplayer;
	public NetworkType NetType { get { return netType; } }
	public INetworkController networkController;

	public void PickNetworkType(int type)
	{
		ApplyNetworkFromInt(type);
		QuitLobbyUI();
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

	void QuitLobbyUI()
	{
		lobbyUI.SetActive(false);
	}
	void ActivateControllers()
	{
		if(this.netType == NetworkType.Singleplayer)
		{
			singlePlayerController.SetActive(true);
			Destroy(P2PController);
			Destroy(ChoseP2P);
			Destroy(serverClientNetworkManager);
		}
		else if(this.netType == NetworkType.ServerClient)
		{
			serverClientNetworkManager.SetActive(true);
			Destroy(P2PController);
			Destroy(ChoseP2P);
			Destroy(singlePlayerController);
		}
		else if(this.netType == NetworkType.P2P)
		{
			P2PController.SetActive(true);
			ChoseP2P.SetActive(true);

			Destroy(serverClientNetworkManager);
			Destroy(singlePlayerController);
		}
		
		gameController.SetActive(true);
	}
}
