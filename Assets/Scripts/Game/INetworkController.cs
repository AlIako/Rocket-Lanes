﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public enum ConsentAction {SpawnRocket};

public interface INetworkController
{
    void Initialize();
    int AskForConsent(ConsentAction consentAction, int[] parameters);
    void ApplyConsent(ConsentAction consentAction, int[] parameters, int consentResult);
}