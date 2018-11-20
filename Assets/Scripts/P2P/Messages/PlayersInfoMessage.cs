using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class PlayersInfoMessage : MessageBase
{
    public int freeLane;
    public List<P2PConnection> connections;

    public override void Deserialize(NetworkReader reader)
    {
        freeLane = Convert.ToInt32(reader.ReadPackedUInt32());
        uint connectionSize = reader.ReadPackedUInt32();
        connections = new List<P2PConnection>();
        for(uint i = 0; i < connectionSize; i++)
        {
            P2PConnection connection = new P2PConnection(   Convert.ToInt32(reader.ReadPackedUInt32()), 
                                                            Convert.ToInt32(reader.ReadPackedUInt32()));

            byte[] bytes = reader.ReadBytesAndSize();
            connection.ip = Encoding.UTF8.GetString(bytes);
            connection.port = Convert.ToInt32(reader.ReadPackedUInt32());
            connections.Add(connection);
        }
    }

    public override void Serialize(NetworkWriter writer)
    {
        writer.WritePackedUInt32(Convert.ToUInt32(freeLane));
        
        writer.WritePackedUInt32(Convert.ToUInt32(connections.Count));
        foreach(P2PConnection connection in connections)
        {
            writer.WritePackedUInt32(Convert.ToUInt32(connection.hostId));
            writer.WritePackedUInt32(Convert.ToUInt32(connection.connectionId));
            writer.WriteBytesFull(Encoding.UTF8.GetBytes(connection.ip));
            writer.WritePackedUInt32(Convert.ToUInt32(connection.port));
        }
    }
	
}
