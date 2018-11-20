using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class P2PListener
{
	public static void Listen()
	{
		int recHostId;
		int connectionId;
		int channelId;
		byte[] recBuffer = new byte[256];
		int bufferSize = 256;
		int dataSize;
		byte error;
		NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
		
		if(recData != NetworkEventType.Nothing)
		{
			Debug.Log("Received: " + recData + ", recHostId: " + recHostId + ", connectionId: " + connectionId + 
						", channelId: " + channelId + ", recBuffer: " + Encoding.UTF8.GetString(recBuffer));
		}
		
		switch (recData)
		{
			case NetworkEventType.Nothing:
				break;
			case NetworkEventType.ConnectEvent:
				P2PConnections.AddConnection(recHostId, connectionId, channelId);
				//buffer = Encoding.UTF8.GetBytes("Hello from " + myPort);
				//When the connection is done, a ConnectEvent is received. Now you can start sending data.
				//NetworkTransport.Send(recHostId, connectionId, myReliableChannelId, buffer, bufferLength, out error);
				//P2PController.CheckError("Send");
				break;
			case NetworkEventType.DataEvent:
				break;
			case NetworkEventType.DisconnectEvent:
				P2PConnections.RemoveConnection(recHostId, connectionId, channelId);
				break;
			case NetworkEventType.BroadcastEvent:
				break;
		}
	}
}
