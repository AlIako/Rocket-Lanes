using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public enum ConsentAction {SpawnRocket, CastShield, JoinGame};

public interface INetworkController
{
    void Initialize();
    void Quit();
    int AskForConsent(ConsentAction consentAction, int[] parameters);
    void ApplyConsent(ConsentAction consentAction, int[] parameters, int consentResult);
    bool HandleCollisions(Lane lane);
}
