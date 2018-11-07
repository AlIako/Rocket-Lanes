using System.Collections;
using System.Collections.Generic;

public enum MessageNature {RequestToJoin, SendPosition, AskContent, ReturnResult, Ack};

public class P2PMessage
{
	public int id = -1; // message id
	public int requestId = -1; // sometimes a message for a same request is sent multiple times
	public int idSender = -1; // who am I?
	public int idReceiver = -1; // to who do I send?
	public MessageNature messageNature; // send position, ask for content or answer a request
	public ConsentAction consentAction; // which request do I send?
	public int answerToRequestId; //to which request do I answer?
	public bool value; //yes or no I agree to a request
}
