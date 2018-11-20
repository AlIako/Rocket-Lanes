using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class P2PSender
{
	public static void Send(int hostId, int connectionId, int channelId, byte[] buffer)
	{
		NetworkTransport.Send(hostId, connectionId, channelId, buffer, P2PController.bufferLength, out P2PController.error);
		P2PController.CheckError("Send");
	}
}
