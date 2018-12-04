using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public enum ConsentAction {SpawnRocket, CastShield, JoinGame};

public interface INetworkController
{
    bool Initialize();
    void Quit();
    void AskForConsent(ConsentMessage consentMessage);
    void ApplyConsent(ConsentMessage consentMessage);
    bool HandleCollisions(Lane lane);
}
