using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class P2PListener
{
	public static P2PController p2PController;
	static int recHostId;
	static int connectionId;
	static int channelId;
	
	public static void Listen()
	{
		byte[] recBuffer = new byte[P2PController.bufferLength];
		int bufferSize = P2PController.bufferLength;
		int dataSize;
		byte error;
		NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
		
		while(recData != NetworkEventType.Nothing)
		{
			//Debug.Log("Received: " + recData + ", recHostId: " + recHostId + ", connectionId: " + connectionId + 
			//			", channelId: " + channelId + ", recBuffer: " + Encoding.UTF8.GetString(recBuffer));
			channelId ++; //get rid of warning
		
			switch (recData)
			{
				case NetworkEventType.Nothing:
					break;
				case NetworkEventType.ConnectEvent:
					P2PConnectionManager.ConnectEvent(recHostId, connectionId);
					break;
				case NetworkEventType.DataEvent:
					CreateNetworkReader(recBuffer);
					break;
				case NetworkEventType.DisconnectEvent:
					P2PConnectionManager.RemoveConnection(recHostId, connectionId);
					break;
				case NetworkEventType.BroadcastEvent:
					break;
			}
			
			recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
		}
	}

	static void CreateNetworkReader(byte[] data)
    {
		//https://docs.unity3d.com/ScriptReference/Networking.NetworkReader.html
        NetworkReader networkReader = new NetworkReader(data);

        // The first two bytes in the buffer represent the size of the message. This is equal to the NetworkReader.Length
        // minus the size of the prefix.
        networkReader.ReadBytes(2);
        //short readerMsgSize = (short)((readerMsgSizeData[1] << 8) + readerMsgSizeData[0]);

        // The message type added in NetworkWriter.StartMessage is to be read now. It is a short and so consists of
        // two bytes. It is the second two bytes on the buffer.
        byte[] readerMsgTypeData = networkReader.ReadBytes(2);
        short readerMsgType = (short)((readerMsgTypeData[1] << 8) + readerMsgTypeData[0]);
        //Debug.Log("Message of type " + readerMsgType + " received");

		if(readerMsgType == MessageTypes.PlayersInfo)
		{
			PlayersInfoMessage message = new PlayersInfoMessage();
			message.Deserialize(networkReader);

			P2PConnectionManager.FetchPlayersInfo(message);
		}
		else if(readerMsgType == MessageTypes.RequestPlayersInfo)
		{
			P2PConnectionManager.SharePlayersInfo(recHostId, connectionId); //share other connections to the new arrived
		}
		else if(readerMsgType == MessageTypes.Position)
		{
			PositionMessage message = new PositionMessage();
			message.Deserialize(networkReader);
			p2PController.ReceivePositionInformation(recHostId, connectionId, message);
		}
    }
}
