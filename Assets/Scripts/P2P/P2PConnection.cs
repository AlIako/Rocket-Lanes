using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class P2PConnection
{
	public int hostId;
	public int connectionId;
	public int channelId; //channelId maybe not defining a connection?

	public P2PConnection(int hostId, int connectionId, int channelId)
	{
		this.hostId = hostId;
		this.connectionId = connectionId;
		this.channelId = channelId;
	}

	public override string ToString()
	{
		return "hostId: " + hostId + ", connectionId: " + connectionId + ", channelId: " + channelId;
	}

	public void Disconnect()
	{
		NetworkTransport.Disconnect(hostId, connectionId, out P2PController.error);
		P2PController.CheckError("Disconnect");
	}
}
