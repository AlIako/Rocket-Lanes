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
	int hostId;
	byte error;
	int connectionId;
	byte[] buffer = new byte[256];
	int bufferLength = 256;

	void Start ()
	{
		gameController = GameObject.FindObjectOfType<GameController>();
	}

	public void NewGame()
	{
		Debug.Log("Starting New P2P Game... port: " + myPort);

		Initialize();
	}

	public void Initialize()
	{
		//https://docs.unity3d.com/Manual/UNetUsingTransport.html
		NetworkTransport.Init();

		config = new ConnectionConfig();
		myReliableChannelId = config.AddChannel(QosType.Reliable);
		//myUnreliableChannelId = config.AddChannel(QosType.Unreliable);

		topology = new HostTopology(config, 3);

		hostId = NetworkTransport.AddHost(topology, myPort);
		Debug.Log("hostId: " + hostId);

		initialized = true;
	}

	public void JoinGame()
	{
		GameObject IPField = GameObject.FindGameObjectWithTag("IPField");
		targetIp = IPField.GetComponent<InputField>().text;

		Debug.Log("Joining P2P Game " + targetIp + ":" + targetPort + "... (my port: " + myPort + ")");

		Initialize();

		connectionId = NetworkTransport.Connect(hostId, targetIp, targetPort, 0, out error);
		Debug.Log("connectionId: " + connectionId);
		CheckError("Connect");
	}

	void OnApplicationQuit()
    {
		NetworkTransport.Disconnect(hostId, connectionId, out error);
		CheckError("Disconnect");
		NetworkTransport.Shutdown();
    }

	void Update()
	{
		if(!initialized)
			return;
		
		int recHostId; 
		int connectionId; 
		int channelId; 
		byte[] recBuffer = new byte[256]; 
		int bufferSize = 256;
		int dataSize;
		byte error;
		NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
		switch (recData)
		{
			case NetworkEventType.Nothing:
				break;
			case NetworkEventType.ConnectEvent:
				buffer = Encoding.UTF8.GetBytes("Hello!");
				//When the connection is done, a ConnectEvent is received. Now you can start sending data.
				NetworkTransport.Send(hostId, connectionId, myReliableChannelId, buffer, bufferLength, out error);
				CheckError("Send");
				break;
			case NetworkEventType.DataEvent:
				break;
			case NetworkEventType.DisconnectEvent:
				break;
			case NetworkEventType.BroadcastEvent:
				break;
		}
		if(recData != NetworkEventType.Nothing)
			Debug.Log("Received: " + recData + ", recHostId: " + recHostId + ", connectionId: " + connectionId + 
						", channelId: " + channelId + ", recBuffer: " + Encoding.UTF8.GetString(recBuffer));
	}

	void CheckError(string label)
	{
        if ((NetworkError)error != NetworkError.Ok)
            Debug.Log(label + " error: " + (NetworkError)error);
        else Debug.Log( label + ": " + (NetworkError)error);
	}

	public void SetMyPort(int port)
	{
		this.myPort = port;
	}

	public void SetTargetPort(int port)
	{
		this.targetPort = port;
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
