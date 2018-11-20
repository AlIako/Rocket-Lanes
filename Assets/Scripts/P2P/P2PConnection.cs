using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class P2PConnection
{
	public int hostId;
	public int connectionId;
	public string ip;
	public int port;
	public int lane = -1;
	bool connectionSuccessful = false;

	public P2PConnection(int hostId, int connectionId, bool fetchConnectionInfo = true)
	{
		this.hostId = hostId;
		this.connectionId = connectionId;
		this.ip = "";
		this.port = 0;

		if(fetchConnectionInfo)
		{
			UnityEngine.Networking.Types.NetworkID netId;
        	UnityEngine.Networking.Types.NodeID nodeId;
			NetworkTransport.GetConnectionInfo(hostId, connectionId, out ip, out port, out netId, out nodeId, out P2PController.error);
		}
	}

	public override string ToString()
	{
		return ip + ":" + port + ", hostId: " + hostId + ", connectionId: " + connectionId;
	}

	public void Disconnect()
	{
		NetworkTransport.Disconnect(hostId, connectionId, out P2PController.error);
		P2PController.CheckError("Disconnect");
	}

	public void SuccessfullyConnect()
	{
		connectionSuccessful = true;
	}

	public bool ConnectionSuccessful()
	{
		return connectionSuccessful;
	}
}
