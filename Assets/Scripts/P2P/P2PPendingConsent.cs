using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2PPendingConsent
{
    public AskConsentMessage askConsentMessage;
    public List<AnswerConsentMessage> answerConsentMessages = new List<AnswerConsentMessage>();

    public P2PPendingConsent(AskConsentMessage message)
    {
        askConsentMessage = message;
    }
}
