using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class P2PConnectionManager
{
	public List<P2PConnection> connections = new List<P2PConnection>();
	public P2PController p2PController;
	public P2PSender p2PSender;
	public P2PChannels p2PChannels;
	public int myHostId;

	public bool requestPlayersInfoSent = false;
	public bool playersInfoReceived = false;

	public void ConnectEvent(int hostId, int connectionId, int channelId)
	{
		P2PConnection connection = GetConnection(hostId, connectionId);

		if(connection == null)
		{
			//new connection from targeted ip or new player
			connection = new P2PConnection(hostId, connectionId);
			connection.SuccessfullyConnect();

			if(p2PController.GameStarted())
			{
				//todo
				//here we would ask consent to others if the newcomer has the right to join the game
				//and compute his lane Id?
			}
			connections.Add(connection);
			Debug.Log("New connection with " + connection);
		}
		else if(!connection.ConnectionSuccessful())
		{
			//successfully connect to an existing player. Connection requested thanks to PlayersInfo
			connection.SuccessfullyConnect();
		}

		//on connecting to an existing game, request infos of other players
		if(!p2PController.GameStarted())
		{
			Debug.Log("Game not started yet");
			if(!requestPlayersInfoSent)
				RequestPlayersInfo(hostId, connectionId, channelId);
			if(playersInfoReceived)
				CheckConnectionsStatus();
		}
		else
		{
			Debug.Log("Game already started for " + p2PController.myPort);
		}
		
	}

	public void SharePlayersInfo(int hostId, int connectionId, int channelId)
	{
		PlayersInfoMessage message = new PlayersInfoMessage();

		Lane freeLane = p2PController.GetGameController().GetFirstUnoccupiedLane();
		if(freeLane == null)
			message.freeLane = 10;
		else message.freeLane = freeLane.id;

		message.connections = new List<P2PConnection>();
		foreach(P2PConnection connection in connections)
		{
			//send all connections except the one with the requester
			if(!(connection.hostId == hostId && connection.connectionId == connectionId))
				message.connections.Add(connection);
		}

		Debug.Log("Sharing players info, count: " + message.connections.Count);
		p2PSender.Send(hostId, connectionId, channelId, message, MessageTypes.PlayersInfo);
	}

	public void FetchPlayersInfo(PlayersInfoMessage message)
	{
		Debug.Log("Fetching players infos..." + message.connections.Count + " my lane: " + message.freeLane);
		playersInfoReceived = true;
		
		p2PController.myLane = message.freeLane;
		if(!(p2PController.myLane >= 0 && p2PController.myLane < 4))
		{
			Debug.Log("Game is full");
			p2PController.myLane = -1;
			p2PController.DisplayError("Game is full");
			return;
		}

		if(connections.Count > 1)
		{
			Debug.Log("Warning! Already have players infos");
		}
		else
		{
			foreach(P2PConnection connection in message.connections)
			{
				Debug.Log("Fetched and request connection with " + connection);

				//send connection requests
				NetworkTransport.Connect(myHostId, connection.ip, connection.port, 0, out P2PController.error);
				P2PController.CheckError("Connect");
			}
		}
		CheckConnectionsStatus();
	}

	public void RequestPlayersInfo(int hostId, int connectionId, int channelId)
	{
		Debug.Log("RequestPlayersInfo");
		requestPlayersInfoSent = true;
		RequestPlayersInfoMessage message = new RequestPlayersInfoMessage();
		p2PSender.Send(hostId, connectionId, channelId, message, MessageTypes.RequestPlayersInfo);
	}

	//if succesfully connected to all, the game can start
	void CheckConnectionsStatus()
	{
		bool connectedToAll = true;
		foreach(P2PConnection connection in connections)
		{
			if(!connection.ConnectionSuccessful())
				connectedToAll = false;
		}

		if(connectedToAll)
		{
			Debug.Log("Connected to All. Can now Start Game");
			p2PController.StartGame();
		}
	}

	public void RemoveConnection(int hostId, int connectionId)
	{
		P2PConnection connection = GetConnection(hostId, connectionId);
		if(connection == null)
			Debug.Log("Warning! Connection with " + hostId + ", " + connectionId + " doesn't exist");
		else
		{
			p2PController.DespawnPlayer(connection.lane);
			connections.Remove(connection);
			Debug.Log("Remove connection with " + connection);
		}
	}

	public void DisconnectAll()
	{
		foreach(P2PConnection connection in connections)
			connection.Disconnect();
		connections.Clear();
	}

	public P2PConnection GetConnection(int hostId, int connectionId)
	{
		P2PConnection connection = connections.FirstOrDefault(c => 
								c.hostId == hostId && 
								c.connectionId == connectionId);
		return connection;
	}

	public void Reset()
	{
		connections = new List<P2PConnection>();
		requestPlayersInfoSent = false;
		playersInfoReceived = false;
	}
}
