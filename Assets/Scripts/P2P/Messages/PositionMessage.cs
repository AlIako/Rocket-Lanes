using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PositionMessage : MessageBase
{
    public uint netId;
    public Vector2 position;

    public override void Deserialize(NetworkReader reader)
    {
        netId = reader.ReadPackedUInt32();
        position = reader.ReadVector2();
    }

    public override void Serialize(NetworkWriter writer)
    {
        writer.WritePackedUInt32(netId);
        writer.Write(position);
    }
}
