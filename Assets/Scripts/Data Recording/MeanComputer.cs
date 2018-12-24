using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MeanComputer : MonoBehaviour
{
    [Serializable]
    public class MeanFile
    {
        public int filesCount = 0;
        public Session sessionTotal = new Session();
        public Session sessionMean = new Session();
    }

    public void ComputeMean()
    {
        ComputeMean("Server");
        ComputeMean("Client");
        ComputeMean("P2P");
        ComputeMean("SinglePlayer");
    }

    public void ComputeMean(string architecture)
    {
        MeanFile meanFile = new MeanFile();

        //read all files
        foreach (string fileName in Directory.GetFiles("Network Data/", architecture + "_*.nd"))
        {
            meanFile.filesCount ++;
            string content = File.ReadAllText(fileName);
            Session session = JsonUtility.FromJson<Session>(content);
            
            meanFile.sessionTotal.networkModel = session.networkModel;
            meanFile.sessionTotal.duration += session.duration;
            meanFile.sessionTotal.averagePlayersCount += session.averagePlayersCount;
            
            meanFile.sessionTotal.messagesSent += session.messagesSent;
            meanFile.sessionTotal.importantMessagesSent += session.importantMessagesSent;
            meanFile.sessionTotal.messagesReceived += session.messagesReceived;
            meanFile.sessionTotal.importantMessagesReceived += session.importantMessagesReceived;
            
            meanFile.sessionTotal.consentSent += session.consentSent;
            meanFile.sessionTotal.averageTimeUntilAnswerForConsent += session.averageTimeUntilAnswerForConsent;
            meanFile.sessionTotal.totalTimeWaitingForConsent += session.totalTimeWaitingForConsent;
            meanFile.sessionTotal.consentTimeOut += session.consentTimeOut;
            meanFile.sessionTotal.cheatsTried += session.cheatsTried;
            meanFile.sessionTotal.cheatsPassed += session.cheatsPassed;
        }

        if(meanFile.filesCount == 0)
            return;

        //compute mean file
            
        meanFile.sessionMean.networkModel = meanFile.sessionTotal.networkModel;
        meanFile.sessionMean.duration = meanFile.sessionTotal.duration / (float)meanFile.filesCount;
        meanFile.sessionMean.averagePlayersCount = meanFile.sessionTotal.averagePlayersCount / (float)meanFile.filesCount;
        
        meanFile.sessionMean.messagesSent = meanFile.sessionTotal.messagesSent / (float)meanFile.filesCount;
        meanFile.sessionMean.importantMessagesSent = meanFile.sessionTotal.importantMessagesSent / (float)meanFile.filesCount;
        meanFile.sessionMean.messagesReceived = meanFile.sessionTotal.messagesReceived / (float)meanFile.filesCount;
        meanFile.sessionMean.importantMessagesReceived = meanFile.sessionTotal.importantMessagesReceived / (float)meanFile.filesCount;
        
        meanFile.sessionMean.consentSent = meanFile.sessionTotal.consentSent / (float)meanFile.filesCount;
        meanFile.sessionMean.averageTimeUntilAnswerForConsent = meanFile.sessionTotal.averageTimeUntilAnswerForConsent / (float)meanFile.filesCount;
        meanFile.sessionMean.totalTimeWaitingForConsent = meanFile.sessionTotal.totalTimeWaitingForConsent / (float)meanFile.filesCount;
        meanFile.sessionMean.consentTimeOut = meanFile.sessionTotal.consentTimeOut / (float)meanFile.filesCount;
        meanFile.sessionMean.cheatsTried = meanFile.sessionTotal.cheatsTried / (float)meanFile.filesCount;
        meanFile.sessionMean.cheatsPassed = meanFile.sessionTotal.cheatsPassed / (float)meanFile.filesCount;

        //write
        string directoryPath = "Network Data/Mean/";
        string fileN = architecture + "_" + DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss.fff") + ".nd";
        string path = directoryPath + fileN;

        string c = JsonUtility.ToJson(meanFile, true);

        Directory.CreateDirectory(directoryPath);
        
        StreamWriter textWriter = new StreamWriter(path);
        textWriter.WriteLine(c);
        textWriter.Close(); 

        Debug.Log("Mean of " + meanFile.filesCount + " files computed: " + path);
    }
}
