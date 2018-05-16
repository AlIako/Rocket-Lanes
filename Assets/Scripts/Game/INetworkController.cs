using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public interface INetworkController
{
    void Initialize();
    void SpawnRocket(int fromPlayerId, int toPlayerId);
}
