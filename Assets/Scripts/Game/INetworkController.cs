using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public enum ConsentAction {SpawnRocket};

public interface INetworkController
{
    void Initialize();
    int AskForConsent(ConsentAction consentAction, int[] parameters);
}
