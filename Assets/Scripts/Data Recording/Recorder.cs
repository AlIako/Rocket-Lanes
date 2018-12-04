using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Recorder : MonoBehaviour
{
    NetworkModel networkModel;
    public static Session session = null;

    float timestampStart;

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
            session = new Session();
            timestampStart = Time.time * 1000.0f;
        }
    }

    public void StopRecording()
    {
        if(RecordingEnabled() && session != null)
        {
            session.date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            session.networkModel = networkModel;
            session.duration = Time.time * 1000.0f - timestampStart;

            WriteSessionFile();

            session = null;
        }
    }

    void WriteSessionFile()
    {
        string directoryPath = "Network Data/";
        string fileName = DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss") + ".nd";
        string path = directoryPath + fileName;

        string content = JsonUtility.ToJson(session, true);

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
