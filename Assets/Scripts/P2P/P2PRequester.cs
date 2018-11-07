using System.Collections;
using System.Collections.Generic;

public class P2PRequester
{
    public static int lastRequestId = 0;



    public static void SendRequest(ConsentAction consentAction)
    {
        foreach(P2PNode node in P2PNodeManager.nodes)
		{
			P2PMessage message = new P2PMessage();
			message.id = P2PMessageSender.NextId();
			message.requestId = NextRequestId();
			
			message.idSender = P2PNodeManager.MyNodeId;
			message.idReceiver = node.id;
			message.messageNature = MessageNature.AskContent;
			message.consentAction = consentAction;
			message.answerToRequestId = -1;
			message.value = false;
		}
    }



    public static int NextRequestId()
    {
        IncrementRequestId();
        return lastRequestId - 1;
    }
    public static void IncrementRequestId()
    {
        lastRequestId ++;
    }

}
