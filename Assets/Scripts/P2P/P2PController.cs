using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class P2PController : MonoBehaviour, INetworkController
{
	[SerializeField]
	Player playerPrefab;

	[HideInInspector]
	public int myPort;

	[HideInInspector]
	public int myLane;
	
	GameController gameController;
	P2PConnectionManager p2PConnectionManager;
	P2PListener p2PListener;
	P2PSender p2PSender;
	P2PChannels p2PChannels;
	
	static ConnectionConfig config = null;
	static HostTopology topology = null;

	string targetIp = "127.0.0.1"; //only when joining
	int targetPort; //only when joining
	bool initialized = false;
	List<Player> players = new List<Player>();

	public static int bufferLength = 1024;
	public static byte error;

	//temp
	float lastSendPosition = 0;
	float cooldownSendPosition = 100;

	void Start()
	{
		gameController = GameObject.FindObjectOfType<GameController>();
		gameController.SetNetworkController(this);
	}

	public void NewGame()
	{
		Debug.Log("Starting New P2P Game... port: " + myPort);
		Initialize();

		StartNewGame();
	}

	public void JoinGame()
	{
		Debug.Log("Joining P2P Game " + targetIp + ":" + targetPort + "... (my port: " + myPort + ")");
		Initialize();

		NetworkTransport.Connect(p2PConnectionManager.myHostId, targetIp, targetPort, 0, out error);
		CheckError("Connect");
	}

	void Update()
	{
		if(!initialized)
			return;
		
		SendPositionInformation();
		p2PListener.Listen();
	}

	public void Initialize()
	{
		p2PChannels = new P2PChannels();
		p2PConnectionManager = new P2PConnectionManager();
		p2PConnectionManager.p2PController = this;
		p2PConnectionManager.p2PChannels = p2PChannels;

		p2PListener = new P2PListener();
		p2PListener.p2PController = this;
		p2PListener.p2PConnectionManager = p2PConnectionManager;

		p2PSender = new P2PSender();
		p2PSender.p2PConnectionManager = p2PConnectionManager;

		p2PConnectionManager.p2PSender = p2PSender;




		//https://docs.unity3d.com/Manual/UNetUsingTransport.html
		NetworkTransport.Init();

		if(config == null)
			config = new ConnectionConfig();
		
		p2PChannels.ReliableChannelId = config.AddChannel(QosType.Reliable);
		p2PChannels.UnreliableChannelId = config.AddChannel(QosType.Unreliable);
		Debug.Log("New ReliableChannelId: " + p2PChannels.ReliableChannelId);
		Debug.Log("New UnreliableChannelId: " + p2PChannels.UnreliableChannelId);

		if(topology == null)
			topology = new HostTopology(config, 10);

		p2PConnectionManager.myHostId = NetworkTransport.AddHost(topology, myPort);

		initialized = true;
	}

	public void StartNewGame()
	{
		myLane = 0;
		StartGame();
	}

	public void StartGame()
	{
		p2PConnectionManager.playersInfoReceived = true;
		p2PConnectionManager.requestPlayersInfoSent = true;

		Player player1 = SpawnPlayer(myLane);
		player1.gameObject.GetComponent<PlayerController>().enabled = true;
		gameController.player = player1;

		gameController.StartGame();
	}

	public void Quit()
	{
		LeaveGame();
	}

	public void LeaveGame()
	{
		initialized = false;
		p2PConnectionManager.DisconnectAll();
		NetworkTransport.Shutdown();

		p2PConnectionManager.Reset();
		players.Clear();
		
		//destroy rockets and players
		Rocket[] rockets = FindObjectsOfType<Rocket>();
		foreach(Rocket rocket in rockets)
			Destroy(rocket.gameObject);
			
		Player[] ps = FindObjectsOfType<Player>();
		foreach(Player p in ps)
			Destroy(p.gameObject);
	}

	public void DisplayError(string er)
	{
		gameController.LeaveGame(false);
		FindObjectOfType<UIController>().DisplayError(er);
	}

	public Player SpawnPlayer(int lane)
	{
		//Debug.Log("Spawn player on lane " + lane);
		Player player = Instantiate(playerPrefab, gameController.GetLane(lane).startPosition.transform.position, Quaternion.identity);
		players.Add(player);
		return player;
	}

	public void DespawnPlayer(int lane)
	{
		Player player = players.FirstOrDefault(p => p.lane.id == lane);
		if(player != null)
			Destroy(player.gameObject);
		players.Remove(player);
	}

	void SendPositionInformation()
	{
		float currentTime = Time.time * 1000;
		if(GameStarted() && currentTime - lastSendPosition > cooldownSendPosition)
		{
			lastSendPosition = Time.time * 1000;
			PositionMessage message = new PositionMessage();
			message.lane = System.Convert.ToUInt32(myLane);
			message.position = gameController.player.transform.position;

			p2PSender.SendToAll(p2PChannels.UnreliableChannelId, message, MessageTypes.Position);
		}
	}

	public void ReceivePositionInformation(int hostId, int connectionId, PositionMessage message)
	{
		int lane = System.Convert.ToInt32(message.lane);
		Player player = players.FirstOrDefault(p => p.lane.id == lane);
		if(player != null)
		{
			player.transform.position = message.position;
		}
		else 
		{
			Debug.Log("New player to spawn");
			SpawnPlayer(lane);
		}

		P2PConnection connection = p2PConnectionManager.GetConnection(hostId, connectionId);
		if(connection != null)
			connection.lane = lane;
	}

	void OnApplicationQuit()
    {
		LeaveGame();
    }

	public static void CheckError(string label)
	{
        if ((NetworkError)error != NetworkError.Ok)
            Debug.Log(label + " error: " + (NetworkError)error);
        //else Debug.Log( label + ": " + (NetworkError)error);
	}

	public GameController GetGameController()
	{
		return gameController;
	}
	public void SetGameController(GameController gameController)
	{
		this.gameController = gameController;
	}

	public bool GameStarted()
	{
		return gameController.gameStarted;
	}

	public void SetMyPort(int port)
	{
		myPort = port;
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
			gameController.GetLane(parameters[1]).spawnManager.Spawn(consentResult);
		}
	}

	public bool HandleCollisions(Lane lane)
	{
		//only if its own lane
		return gameController.player.lane.id == lane.id;
	}
}
