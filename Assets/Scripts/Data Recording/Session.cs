using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NetworkModel {SinglePlayer, ServerClient, P2P};

//sessions starts when joining/starting a game. stops when leaving.
//https://docs.unity3d.com/Manual/JSONSerialization.html
[Serializable]
public class Session
{
    public string date = "";
    public NetworkModel networkModel;
    //public int port = 0;
    //public string ip = "";
    public float duration = 0;
    public float averagePlayersCount = 0;

    public int messagesSent = 0;
    public int importantMessagesSent = 0;
    public int messagesReceived = 0;
    public int importantMessagesReceived = 0;
    public float incomingBandwith = 0; //size in mb
    public float leavingBandwith = 0; //size in mb

    //P2P only
    public float averageTimeUntilAnswerForConsent = 0;

    public int cheatsTried = 0;
    public int cheatsPassed = 0;
}
