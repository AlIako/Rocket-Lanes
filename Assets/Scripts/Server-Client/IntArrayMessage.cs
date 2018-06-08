using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class IntArrayMessage : MessageBase
{
	public ConsentAction consentAction;
	public int[] parameters;

    public override void Deserialize(NetworkReader reader)
    {
		int arraySize = reader.ReadInt32();
		parameters = new int[arraySize];
		for(int i = 0; i < arraySize; i ++)
			parameters[i] = reader.ReadInt32();
    }

    public override void Serialize(NetworkWriter writer)
    {
        writer.Write(parameters.Length);
		for(int i = 0; i < parameters.Length; i ++)
		{
        	writer.Write(parameters[i]);
		}
    }
}
