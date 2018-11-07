using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class P2PMessageSender
{
    public static int lastId = 0;
    static List<P2PMessageToSend> messagesToSend;
    int timeBetweenRetrySending = 5;

    public void Update()
    {
        foreach(P2PMessageToSend messageToSend in messagesToSend)
        {
            if(messageToSend.ReadyToSend(timeBetweenRetrySending))
                messageToSend.Send();
        }
    }
    public static void SendMessage(P2PMessage message)
    {
        messagesToSend.Add(new P2PMessageToSend(message));
    }

    public static void AcknowledgementMessage(P2PMessage message)
    {
        P2PMessageToSend messageToDelete = null;
        if(message.requestId != -1)
        {
            // find a messageToSend with requestId == message.requestId and idReceiver == message.idSender
            messageToDelete = messagesToSend.FirstOrDefault(mts => mts.message.requestId == message.requestId && 
                                                            mts.message.idReceiver == message.idSender);

        }
        else if(message.answerToRequestId != -1)
        {
            // find a messageToSend with answerToRequestId == message.answerToRequestId and idReceiver == message.idSender
            messageToDelete = messagesToSend.FirstOrDefault(mts => mts.message.answerToRequestId == message.answerToRequestId && 
                                                            mts.message.idReceiver == message.idSender);
        }

        if(messageToDelete != null)
        {
            // message targeted from ACK found, delete it
            messagesToSend.Remove(messageToDelete);
        }
    }


    public static int NextId()
    {
        IncrementMessageId();
        return lastId - 1;
    }
    public static void IncrementMessageId()
    {
        lastId ++;
    }
}
