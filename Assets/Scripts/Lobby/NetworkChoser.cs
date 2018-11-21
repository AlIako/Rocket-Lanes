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

	private GameObject currentController = null;

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

	public void EnterLobbyUI()
	{
		lobbyUI.SetActive(true);
		DeactivateControllers();
	}

	void QuitLobbyUI()
	{
		lobbyUI.SetActive(false);
	}

	void ActivateControllers()
	{
		if(this.netType == NetworkType.Singleplayer)
		{
			currentController = Instantiate(singlePlayerController);
		}
		else if(this.netType == NetworkType.ServerClient)
		{
			currentController = Instantiate(serverClientNetworkManager);
		}
		else if(this.netType == NetworkType.P2P)
		{
			currentController = Instantiate(P2PController);
			ChoseP2P.SetActive(true);
		}
		
		gameController.GetComponent<GameController>().networkController = currentController.GetComponent<INetworkController>();
	}

	void DeactivateControllers()
	{
		if(currentController != null)
		{
			Destroy(currentController.gameObject);
			currentController = null;
			ChoseP2P.SetActive(false);
		}
	}
}
