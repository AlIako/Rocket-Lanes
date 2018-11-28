using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class P2PConnection
{
	public int hostId = -1;
	public int connectionId = -1;
	public string ip;
	public int port;
	public int lane = 10;
	bool connectionSuccessful = false;

	public P2PConnection(int hostId, int connectionId)
	{
		this.hostId = hostId;
		this.connectionId = connectionId;
		this.ip = "";
		this.port = 0;
	}

	public P2PConnection(string ip, int port)
	{
		this.hostId = -1;
		this.connectionId = -1;
		this.ip = ip;
		this.port = port;
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
