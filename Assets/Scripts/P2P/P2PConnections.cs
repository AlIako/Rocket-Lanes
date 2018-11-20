using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class P2PConnections
{
	static List<P2PConnection> connections = new List<P2PConnection>();

	public static void ShareConnections(int hostId, int connectionId)
	{
		byte[] buffer = new byte[P2PController.bufferLength];
		buffer = System.Text.Encoding.UTF8.GetBytes("Hello, here are informations about other connections: " + connections.Count);
		P2PSender.Send(hostId, connectionId, P2PChannels.ReliableChannelId, buffer);
	}

	public static void AddConnection(int recHostId, int connectionId, int channelId)
	{
		P2PConnection connection = connections.FirstOrDefault(c => 
								c.hostId == recHostId && 
								c.connectionId == connectionId &&
								c.channelId == channelId);
		if(connection == null)
		{
			//new connection
			connection = new P2PConnection(recHostId, connectionId, channelId);
			connections.Add(connection);
			Debug.Log("New connection with " + connection);
		}
		else
			Debug.Log("Warning! Connection with " + connection + " already exists");
	}

	public static void RemoveConnection(int recHostId, int connectionId, int channelId)
	{
		P2PConnection connection = connections.FirstOrDefault(c => 
								c.hostId == recHostId && 
								c.connectionId == connectionId &&
								c.channelId == channelId);
		if(connection == null)
			Debug.Log("Warning! Connection with " + connection + " doesn't exist");
		else
		{
			//Remove connection
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
}
