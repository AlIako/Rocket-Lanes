using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class P2PController : MonoBehaviour, INetworkController
{
	int myPort;
	string targetIp; //only when joining
	int targetPort; //only when joining
	bool initialized = false;

	static GameController gameController;
	public static int bufferLength = 1024;
	public static byte error;

	public void NewGame()
	{
		Debug.Log("Starting New P2P Game... port: " + myPort);
		Initialize();

		StartGame();
	}

	public void JoinGame()
	{
		Debug.Log("Joining P2P Game " + targetIp + ":" + targetPort + "... (my port: " + myPort + ")");
		Initialize();

		NetworkTransport.Connect(P2PConnections.myHostId, targetIp, targetPort, 0, out error);
		CheckError("Connect");
	}

	void Update()
	{
		if(!initialized)
			return;

		P2PListener.Listen();
	}

	public void Initialize()
	{
		//https://docs.unity3d.com/Manual/UNetUsingTransport.html
		NetworkTransport.Init();

		ConnectionConfig config = new ConnectionConfig();
		P2PChannels.ReliableChannelId = config.AddChannel(QosType.Reliable);
		P2PChannels.UnreliableChannelId = config.AddChannel(QosType.Unreliable);

		HostTopology topology = new HostTopology(config, 10);

		P2PConnections.myHostId = NetworkTransport.AddHost(topology, myPort);
		//Debug.Log("myHostId: " + P2PConnections.myHostId);

		initialized = true;
	}

	public static void StartGame()
	{
		P2PConnections.playersInfoReceived = true;
		P2PConnections.requestPlayersInfoSent = true;
		
		if(P2PController.gameController == null)
			P2PController.gameController = GameObject.FindObjectOfType<GameController>();
		P2PController.gameController.StartGame();
	}

	void OnApplicationQuit()
    {
		P2PConnections.DisconnectAll();
		NetworkTransport.Shutdown();
    }

	public static void CheckError(string label)
	{
        if ((NetworkError)error != NetworkError.Ok)
            Debug.Log(label + " error: " + (NetworkError)error);
        else Debug.Log( label + ": " + (NetworkError)error);
	}

	void Awake()
	{
		P2PController.gameController = GameObject.FindObjectOfType<GameController>();
	}

	public static bool GameStarted()
	{
		return GameController.gameStarted;
	}

	public void SetMyPort(int port)
	{
		this.myPort = port;
	}

	public void SetTargetPort(int port)
	{
		this.targetPort = port;
	}

	public void SetTargetIp(string ip)
	{
		this.targetIp = ip;
	}



    public int AskForConsent(ConsentAction consentAction, int[] parameters)
	{
		if(consentAction == ConsentAction.SpawnRocket)
		{
			//Vote necessary! Pick majority
		}
		return -1;
	}

    public void ApplyConsent(ConsentAction consentAction, int[] parameters, int consentResult)
	{
		if(consentAction == ConsentAction.SpawnRocket)
		{
			gameController.lanes[parameters[1]].spawnManager.Spawn(consentResult);
		}
	}

	public bool HandleCollisions(Lane lane)
	{
		//only if its own lane
		return gameController.player.lane.id == lane.id;
	}
}
