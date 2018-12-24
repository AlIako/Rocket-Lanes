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
    public string networkModel;
    //public int port = 0; //interesting but useless
    //public string ip = ""; //interesting but useless
    public float duration = 0;
    public float averagePlayersCount = 0;

    public float messagesSent = 0;
    public float importantMessagesSent = 0;
    public float messagesReceived = 0;
    public float importantMessagesReceived = 0;
    //public float incomingBandwith = 0; //size in mb. Not conclusive, better to use external software!
    //public float leavingBandwith = 0; //size in mb. Not conclusive, better to use external software!

    //P2P only
    public float consentSent = 0;
    public float averageTimeUntilAnswerForConsent = 0;
    public float totalTimeWaitingForConsent = 0;
    public float consentTimeOut = 0;
    public float cheatsTried = 0;
    public float cheatsPassed = 0;


    //non serialized
    [NonSerialized]
    float startTime;
    
    [NonSerialized]
    int lastPlayersCount = 0;
    
    [NonSerialized]
    float lastPlayersCountTime = 0;

    public Session(string networkModel = "")
    {
        this.networkModel = networkModel;
    }

    public void Start()
    {
        startTime = Time.time * 1000.0f;
    }

    public void Stop()
    {
        duration = Time.time * 1000.0f - startTime;
        date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        UpdatePlayersCount(lastPlayersCount);
    }

    public void AddSentAndAppliedConsent(float timeToConsent)
    {
        Debug.Log("AddSentAndAppliedConsent, " + (Time.time * 1000.0f - timeToConsent) + " ms");
        consentSent ++;
        totalTimeWaitingForConsent += Time.time * 1000.0f - timeToConsent;
        Debug.Log("totalTimeWaitingForConsent, " + totalTimeWaitingForConsent + " ms");
        averageTimeUntilAnswerForConsent = totalTimeWaitingForConsent / consentSent;
    }

    public void UpdatePlayersCount(int playersCount)
    {
        float currentTime = Time.time * 1000.0f - startTime;
        if(lastPlayersCountTime == 0)
        {
            averagePlayersCount = playersCount;
        }
        else
        {
            float untilLastCountWeight = lastPlayersCountTime / currentTime;
            float lastCountUntilNowWeight = (currentTime - lastPlayersCountTime) / currentTime;

            averagePlayersCount = untilLastCountWeight * averagePlayersCount + lastCountUntilNowWeight * lastPlayersCount;
        }
        
        lastPlayersCount = playersCount;
        lastPlayersCountTime = currentTime;
    }

    public void AddIncomingBandwidth(int data)
    {
        //incomingBandwith += data;
    }

    public void AddLeavingBandwidth(int data)
    {
        //leavingBandwith += data;
    }
}
