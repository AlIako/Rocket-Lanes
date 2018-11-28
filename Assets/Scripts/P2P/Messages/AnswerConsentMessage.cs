using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class AnswerConsentMessage : MessageBase
{
    public int consentId;
    public ConsentAction consentAction;
    public int result;
    public List<int> parameters = new List<int>();

    
    public override void Deserialize(NetworkReader reader)
    {
        consentId = Convert.ToInt32(reader.ReadPackedUInt32());
        consentAction = (ConsentAction)reader.ReadPackedUInt32();
        result = Convert.ToInt32(reader.ReadPackedUInt32());
        parameters = new List<int>();
        uint parameterSize = reader.ReadPackedUInt32();
        for(uint i = 0; i < parameterSize; i ++)
        {
            parameters.Add(Convert.ToInt32(reader.ReadPackedUInt32()));
        }
    }

    public override void Serialize(NetworkWriter writer)
    {
        writer.WritePackedUInt32(Convert.ToUInt32(consentId));
        writer.WritePackedUInt32(Convert.ToUInt32(consentAction));
        writer.WritePackedUInt32(Convert.ToUInt32(result));
        writer.WritePackedUInt32(Convert.ToUInt32(parameters.Count));
        foreach(int parameter in parameters)
        {
            writer.WritePackedUInt32(Convert.ToUInt32(parameter));
        }
    }
}
