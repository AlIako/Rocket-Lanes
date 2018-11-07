using System.Collections;
using System.Collections.Generic;


// handles incoming request
// computes answers and send them
public class P2PIncomingMessagesHandler
{

	// callback triggered on message received
	void OnMessageReceived(P2PMessage message)
	{
		// handle request to join
		if(message.messageNature == MessageNature.RequestToJoin)
		{
			if(message.idReceiver == -1) // request to join, otherwise its an answer to a request to join
			{
				// for the future: send ack right away ? computation and consent can take some time

				// if alone in a game and oneself sill has no idea (same? idk), compute an idea for oneself
				if(P2PNodeManager.StillOffline())
					P2PNodeManager.ComputeIdForMyself();

				P2PMessage answer = InitializeJoinAnswerMessage();
				// check if there is room for a new player
				if(!P2PNodeManager.GameFull())
				{
					// compute an id for new node
					int newId = P2PNodeManager.ComputeIdForNewPlayer();

					// send the id back
					answer.idReceiver = newId;

					// for now, simply agree to each request
					answer.value = true;
				}
				else
				{
					answer.value = false;
				}

				P2PMessageSender.SendMessage(answer);

				// need ack!
				// upon receiving ack, add this player to nodelist. he has succesfully joined!
					
			}
			else 
			{
				// i received my id!
				P2PNodeManager.MyNodeId = message.idReceiver;

				// now i actually need the ids of the other nodes ...

				// for now, send ack
				P2PMessage answer = InitializeAcknowledgementMessage(message);
				P2PMessageSender.SendMessage(answer);
			}

		}
		else if(message.idReceiver == P2PNodeManager.MyNodeId) // am I the right recipient?
		{
			if(message.messageNature == MessageNature.AskContent)
			{
				// create an answer and send
				P2PMessage answer = InitializeAnswerMessage(message);

				// for now, simply agree to each request
				answer.value = true;
				P2PMessageSender.SendMessage(answer);
			}
			else if(message.messageNature == MessageNature.ReturnResult)
			{
				// fetch and add to all results to this request

				// remove messageToSend to corresponding client. This acts as an ACK
				P2PMessageSender.AcknowledgementMessage(message);
			}
			else if(message.messageNature == MessageNature.SendPosition)
			{
				// apply position to the right player
			}
			else if(message.messageNature == MessageNature.Ack)
			{
				// acknowledgement for Result
				P2PMessageSender.AcknowledgementMessage(message);
			}
		}
	}

	P2PMessage InitializeAnswerMessage(P2PMessage message)
	{
		P2PMessage answer = new P2PMessage();
		answer.id = P2PMessageSender.NextId();
		answer.requestId = -1;
		answer.idSender = P2PNodeManager.MyNodeId;
		answer.idReceiver = message.idSender;
		answer.messageNature = MessageNature.ReturnResult;
		answer.consentAction = message.consentAction;
		answer.answerToRequestId = message.requestId;
		answer.value = false;

		return answer;
	}

	P2PMessage InitializeJoinAnswerMessage()
	{
		P2PMessage answer = new P2PMessage();
		answer.id = P2PMessageSender.NextId();
		answer.requestId = -1;
		answer.idSender = P2PNodeManager.MyNodeId;
		answer.messageNature = MessageNature.ReturnResult;

		return answer;
	}

	P2PMessage InitializeAcknowledgementMessage(P2PMessage message)
	{
		P2PMessage answer = new P2PMessage();
		answer.id = P2PMessageSender.NextId();
		answer.requestId = -1;
		answer.idSender = P2PNodeManager.MyNodeId;
		answer.idReceiver = message.idSender;
		answer.messageNature = MessageNature.Ack;
		answer.consentAction = message.consentAction;
		answer.answerToRequestId = message.requestId;
		answer.value = true;

		return answer;
	}
}
