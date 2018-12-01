using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PositionMessage : MessageBase
{
    public uint lane;
    public Vector2 position;
    public uint hp;

    public override void Deserialize(NetworkReader reader)
    {
        lane = reader.ReadPackedUInt32();
        position = reader.ReadVector2();
        hp = reader.ReadPackedUInt32();
    }

    public override void Serialize(NetworkWriter writer)
    {
        writer.WritePackedUInt32(lane);
        writer.Write(position);
        writer.WritePackedUInt32(hp);
    }
}
