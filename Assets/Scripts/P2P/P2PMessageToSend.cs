using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2PMessageToSend
{
	public P2PMessage message;
	float lastTimestampSent;

	public P2PMessageToSend(P2PMessage message)
	{
		this.message = message;
		int lastTimestampSent = 0;
	}

	public bool ReadyToSend(int cooldown)
	{
		if(lastTimestampSent == 0)
			return true;
		if(Time.time - lastTimestampSent > cooldown)
			return true;
		return false;
	}

	public void Send()
	{
		lastTimestampSent = Time.time;

		// send via udp to the right client
	}
}
