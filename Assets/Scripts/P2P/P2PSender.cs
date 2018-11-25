using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class P2PSender
{
	public P2PConnectionManager p2PConnectionManager;
	public void Send(int hostId, int connectionId, int channelId, MessageBase message, short messageType)
	{
		NetworkWriter writer = new NetworkWriter();
		writer.StartMessage(messageType);
		message.Serialize(writer);
		writer.FinishMessage();
		byte[] writerData = writer.ToArray();

		NetworkTransport.Send(hostId, connectionId, channelId, writerData, P2PController.bufferLength, out P2PController.error);
		P2PController.CheckError("Send");
	}

	public void SendToAll(int channelId, MessageBase message, short messageType)
	{
		foreach(P2PConnection connection in p2PConnectionManager.connections)
			Send(connection.hostId, connection.connectionId, channelId, message, messageType);
	}
}
