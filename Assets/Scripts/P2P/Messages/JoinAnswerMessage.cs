using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class JoinAnswerMessage : MessageBase
{
    public int lane;
    public List<P2PConnection> successfulConnections = new List<P2PConnection>();
    public int r, g, b;

    public override void Deserialize(NetworkReader reader)
    {
        lane = Convert.ToInt32(reader.ReadPackedUInt32());
        int successfulConnectionsCount = Convert.ToInt32(reader.ReadPackedUInt32());
        for(int i = 0; i < successfulConnectionsCount; i ++)
        {
            byte[] bytes = reader.ReadBytesAndSize();
            string ip = Encoding.UTF8.GetString(bytes);
            int port = Convert.ToInt32(reader.ReadPackedUInt32());
            
            P2PConnection connection = new P2PConnection(ip, port);
            connection.lane = Convert.ToInt32(reader.ReadPackedUInt32());
            
            successfulConnections.Add(connection);
        }
        r = Convert.ToInt32(reader.ReadPackedUInt32());
        g = Convert.ToInt32(reader.ReadPackedUInt32());
        b = Convert.ToInt32(reader.ReadPackedUInt32());
    }

    public override void Serialize(NetworkWriter writer)
    {
        writer.WritePackedUInt32(Convert.ToUInt32(lane));
        writer.WritePackedUInt32(Convert.ToUInt32(successfulConnections.Count));
        foreach(P2PConnection connection in successfulConnections)
        {
            writer.WriteBytesFull(Encoding.UTF8.GetBytes(connection.ip));
            writer.WritePackedUInt32(Convert.ToUInt32(connection.port));
            writer.WritePackedUInt32(Convert.ToUInt32(connection.lane));
        }
        writer.WritePackedUInt32(Convert.ToUInt32(r));
        writer.WritePackedUInt32(Convert.ToUInt32(g));
        writer.WritePackedUInt32(Convert.ToUInt32(b));
    }
}
