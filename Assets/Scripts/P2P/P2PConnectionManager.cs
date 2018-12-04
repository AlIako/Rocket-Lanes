using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class P2PConnectionManager
{
	public static List<P2PConnection> connections = new List<P2PConnection>();
	public static P2PController p2PController;
	public static int myHostId = -1;

	public static bool JoinRequestSend = false;
	public static bool JoinAnswerReceived = false;

	public static float connectionRequestSentTime = 0;

	public static int SuccessfulConnectionsCount()
	{
		int result = 0;
		foreach(P2PConnection connection in connections)
		{
			if(connection.ConnectionSuccessful())
				result ++;
		}
		return result;
	}

	public static List<P2PConnection> GetSuccessfulConnections()
	{
		List<P2PConnection> successfulConnections = new List<P2PConnection>();
		foreach(P2PConnection connection in connections)
		{
			if(connection.ConnectionSuccessful())
				successfulConnections.Add(connection);
		}
		return successfulConnections;
	}

	public static void OnJoinRequest(int hostId, int connectionId, JoinRequestMessage message)
	{
		Debug.Log("JoinRequest received");
		//ask others if a player can join to that lane
		ConsentMessage consentMessage = new ConsentMessage();
		consentMessage.consentAction = ConsentAction.JoinGame;
		Lane freeLane = p2PController.GetGameController().GetFirstUnoccupiedLane();
		consentMessage.result = 10;
		if(freeLane != null)
			consentMessage.result = freeLane.id;

		consentMessage.parameters.Add(hostId);
		consentMessage.parameters.Add(connectionId);

		p2PController.AskForConsent(consentMessage);
	}

	public static void OnJoinAnswer(int hostId, int connectionId, JoinAnswerMessage message)
	{
		if(p2PController.GameStarted() || JoinAnswerReceived)
			return;
		
		Debug.Log("JoinAnswer received, lane: " + message.lane + ", playersCount: " + message.successfulConnections.Count);
		if(!(message.lane >= 0 && message.lane < 4))
		{
			Debug.Log("Game is full");
			p2PController.myLane = -1;
			p2PController.DisplayError("Game is full");
			return;
		}
		else
		{
			Debug.Log("allowed to join the game! Now need to connect to all players");
			JoinAnswerReceived = true;

			p2PController.myLane = message.lane;
			
			foreach(P2PConnection connection in message.successfulConnections)
			{
				connections.Add(connection);
				NetworkTransport.Connect(myHostId, connection.ip, connection.port, 0, out P2PController.error);
				P2PController.CheckError("Connect");
			}
			
			P2PConnectionManager.GetConnection(hostId, connectionId).SuccessfullyConnect();

			CheckConnectionsStatus();
		}
	}

	public static void OnJoinAnnounce(int hostId, int connectionId, JoinAnnounceMessage message)
	{
		Debug.Log("OnJoinAnnounce received");
		P2PConnection connection = P2PConnectionManager.GetConnection(hostId, connectionId);
		connection.SuccessfullyConnect();
	}

	public static void ConnectEvent(int hostId, int connectionId)
	{
		int port;
		string ip;
		UnityEngine.Networking.Types.NetworkID netId;
		UnityEngine.Networking.Types.NodeID nodeId;
		NetworkTransport.GetConnectionInfo(hostId, connectionId, out ip, out port, out netId, out nodeId, out P2PController.error);

		P2PConnection connection = P2PConnectionManager.GetConnection(ip, port);

		if(connection == null)
		{
			//new connection from targeted ip or new player
			connection = new P2PConnection(hostId, connectionId);
			connection.ip = ip;
			connection.port = port;
			connections.Add(connection);
			Debug.Log("New connection with " + connection);

			if(!JoinRequestSend) //I'm wanting to join
			{
				Debug.Log("Sending Join Request");
				JoinRequestSend = true;
				JoinRequestMessage message = new JoinRequestMessage();
				P2PSender.Send(hostId, connectionId, P2PChannels.ReliableChannelId, message, MessageTypes.JoinRequest);
			}
			else if(JoinAnswerReceived && !p2PController.GameStarted())
				connection.SuccessfullyConnect();
		}
		else if(!connection.ConnectionSuccessful())
		{
			//successfully connect to an existing player. Connection requested previously
			connection.hostId = hostId;
			connection.connectionId = connectionId;
			connection.SuccessfullyConnect();

			JoinAnnounceMessage announceMessage = new JoinAnnounceMessage();
			P2PSender.Send(hostId, connectionId, P2PChannels.ReliableChannelId, announceMessage, MessageTypes.JoinAnnounce);
		}

		if(!p2PController.GameStarted())
		{
			if(JoinAnswerReceived)
				CheckConnectionsStatus();
		}
		
	}

	//if succesfully connected to all, the game can start
	static void CheckConnectionsStatus()
	{
		if(myHostId == -1)
			return;
		
		bool connectedToAll = true;
		foreach(P2PConnection connection in connections)
		{
			if(!connection.ConnectionSuccessful())
				connectedToAll = false;
		}

		if(connectedToAll)
			p2PController.StartGame();
	}

	public static void RemoveConnection(int hostId, int connectionId)
	{
		P2PConnection connection = P2PConnectionManager.GetConnection(hostId, connectionId);
		if(connection == null)
			Debug.Log("Warning! Connection with " + connection + " doesn't exist");
		else
		{
			p2PController.DespawnPlayer(connection.lane);
			connections.Remove(connection);
			Debug.Log("Remove connection with " + connection);
		}
	}

	public static void DisconnectAll()
	{
		foreach(P2PConnection connection in connections)
			connection.Disconnect();
		connections.Clear();
	}

	public static P2PConnection GetConnection(int hostId, int connectionId)
	{
		P2PConnection connection = connections.FirstOrDefault(c => 
								c.hostId == hostId && 
								c.connectionId == connectionId);
		return connection;
	}

	public static P2PConnection GetConnection(string ip, int port)
	{
		P2PConnection connection = connections.FirstOrDefault(c => 
								c.ip == ip && 
								c.port == port);
		return connection;
	}

	public static void Reset()
	{
		connections = new List<P2PConnection>();
		myHostId = -1;
		JoinRequestSend = false;
		JoinAnswerReceived = false;
	}
}
