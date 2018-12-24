using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Recorder : MonoBehaviour
{
    string networkModel;
    public static Session session = null;

    void Start()
    {
        if(GameObject.FindObjectOfType<SinglePlayerController>())
            networkModel = "SinglePlayer";
        else if(GameObject.FindObjectOfType<MyNetworkManager>())
            networkModel = "Server-Client";
        else if(GameObject.FindObjectOfType<P2PController>())
            networkModel = "P2P";
    }

    public void StartRecording()
    {
        if(RecordingEnabled())
        {
            session = new Session(networkModel);
            session.Start();
        }
    }

    public void StopRecording()
    {
        if(RecordingEnabled() && session != null)
        {
            session.Stop();
            WriteSessionFile(session);
            session = null;
        }
    }

    public void UpdatePlayersCount(int playersCount)
    {
        if(RecordingEnabled() && session != null)
        {
            session.UpdatePlayersCount(playersCount);
        }
    }

    void WriteSessionFile(Session sessionToWrite)
    {
        //Debug.Log("Write.. " + sessionToWrite.totalTimeWaitingForConsent);
        if(sessionToWrite.averagePlayersCount < 1)
            return;

        string directoryPath = "Network Data/";
        string fileName = "";
        if(networkModel == "Server-Client")
        {
            if(GameObject.FindObjectOfType<MyNetworkManager>().IsServer())
                fileName = "Server_";
            else fileName = "Client_";
        }
        else fileName = networkModel + "_";
        fileName += DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss.fff") + ".nd";


        string path = directoryPath + fileName;

        string content = JsonUtility.ToJson(sessionToWrite, true);

        Directory.CreateDirectory(directoryPath);
        
        StreamWriter textWriter = new StreamWriter(path);
        textWriter.WriteLine(content);
        textWriter.Close(); 
    }

    public bool RecordingEnabled()
    {
        return PlayerPrefs.GetInt("Recording", 0) == 1;
    }
}
