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
	GameController gameController;
	string targetIp;
	int myPort;
	int targetPort;

	bool initialized = false;

	int myReliableChannelId;
	//int myUnreliableChannelId;
	ConnectionConfig config;
	HostTopology topology;
	int myHostId;
	
	public static byte error;

	public void NewGame()
	{
		Debug.Log("Starting New P2P Game... port: " + myPort);
		Initialize();
	}

	public void JoinGame()
	{
		Debug.Log("Joining P2P Game " + targetIp + ":" + targetPort + "... (my port: " + myPort + ")");
		Initialize();

		NetworkTransport.Connect(myHostId, targetIp, targetPort, 0, out error);
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

		config = new ConnectionConfig();
		myReliableChannelId = config.AddChannel(QosType.Reliable);
		//myUnreliableChannelId = config.AddChannel(QosType.Unreliable);

		topology = new HostTopology(config, 10);

		myHostId = NetworkTransport.AddHost(topology, myPort);
		Debug.Log("myHostId: " + myHostId);

		initialized = true;
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

	void Start()
	{
		gameController = GameObject.FindObjectOfType<GameController>();
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
