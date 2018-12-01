using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class P2PConsentManager
{
    public static P2PController p2PController;

    public static int nextConsentId = 0;
    static List<P2PPendingConsent> pendingConsents = new List<P2PPendingConsent>();

    public static void CheckForTimeoutPendingConsents()
    {
        float currentTime = Time.time * 1000.0f;
        foreach(P2PPendingConsent pendingConsent in pendingConsents)
        {
            //Debug.Log("current Time: " + currentTime + ", pendingConsent.pendingSince: " + pendingConsent.pendingSince +
            //            ", p2PController.requestTimeoutTime: " + p2PController.requestTimeoutTime + "dif: " + 
            //            (currentTime - pendingConsent.pendingSince));
            if(currentTime - pendingConsent.pendingSince > p2PController.requestTimeoutTimeMs)
            {
                Debug.Log("Request time out!");
                if(pendingConsent.answerConsentMessages.Count > 1)
                    ApplyPendingConsent(pendingConsent);
                else
                {
                    Debug.Log("Not enough votes! (" + pendingConsent.answerConsentMessages.Count + "). Canceling request.");
                    pendingConsents.Remove(pendingConsent);
                }
                break;
            }
        }
    }

    public static void ApplyPendingConsent(P2PPendingConsent pendingConsent)
    {
        Debug.Log("Received enough votes! (" + (pendingConsent.answerConsentMessages.Count - 1)+ ").");
        //enough votes received! Pick the most occuring result
        int mostOccuringAnswerResult = pendingConsent.answerConsentMessages
                                                                .GroupBy(acm => acm.result)
                                                                .OrderByDescending(g => g.Count())
                                                                .Select(g => g.Key)
                                                                .FirstOrDefault();
        Debug.Log("Most occuring vote result: " + mostOccuringAnswerResult);

        //apply it
        AnswerConsentMessage mostOccuringAnswer = pendingConsent.answerConsentMessages[0];
        mostOccuringAnswer.result = mostOccuringAnswerResult;
        ApplyAndSpreadConsentResult(mostOccuringAnswer);

        pendingConsents.Remove(pendingConsent);
    }

    public static void AddPendingConsent(AskConsentMessage message)
    {
        pendingConsents.Add(new P2PPendingConsent(message));
    }
    public static void ReceiveAnswerConsent(AnswerConsentMessage message)
    {
        Debug.Log("ReceiveAnswerConsent for " + message.consentAction + ", id: " + message.consentId);
        int consentId = message.consentId;
        P2PPendingConsent pendingConsent = pendingConsents.FirstOrDefault(p => p.askConsentMessage.consentId == consentId);
        if(pendingConsent != null)
        {
            pendingConsent.answerConsentMessages.Add(message);
            if(pendingConsent.answerConsentMessages.Count >= P2PConnectionManager.connections.Count - 1)
                ApplyPendingConsent(pendingConsent);
        }
        else
        {
            Debug.Log("Error: receive answer consent for unsent consent request");
        }
    }

    public static void ApplyAndSpreadConsentResult(AnswerConsentMessage message)
    {
        ApplyConsentMessage applyMessage = new ApplyConsentMessage();
        applyMessage.consentAction = message.consentAction;
        applyMessage.result = message.result;
        applyMessage.parameters = message.parameters;

        P2PSender.SendToAll(P2PChannels.ReliableChannelId, applyMessage, MessageTypes.ApplyConsent);
        p2PController.OnApplyConsentMsg(applyMessage);
    }

    public static void Reset()
    {
        nextConsentId = 0;
        pendingConsents.Clear();
    }

    public static int GetNextConsentIdAndIncrement()
    {
        int result = nextConsentId;
        nextConsentId ++;
        return result;
    }
}
