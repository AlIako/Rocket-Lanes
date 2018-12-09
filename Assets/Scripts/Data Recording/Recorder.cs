using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Recorder : MonoBehaviour
{
    NetworkModel networkModel;
    public static Session session = null;

    void Start()
    {
        if(GameObject.FindObjectOfType<SinglePlayerController>())
            SetNetworkModel(NetworkModel.SinglePlayer);
        else if(GameObject.FindObjectOfType<MyNetworkManager>())
            SetNetworkModel(NetworkModel.ServerClient);
        else if(GameObject.FindObjectOfType<P2PController>())
            SetNetworkModel(NetworkModel.P2P);
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
        string directoryPath = "Network Data/";
        string fileName = DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss") + ".nd";
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

    void SetNetworkModel(NetworkModel networkModel)
    {
        this.networkModel = networkModel;
    }
}
