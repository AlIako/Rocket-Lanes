using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2PController : MonoBehaviour, INetworkController
{
	void Start ()
	{
		
	}

	public void Initialize()
	{

	}
	
    public int AskForConsent(ConsentAction consentAction, int[] parameters)
	{
		//Vote necessary! Pick majority
		return 1;
	}
}
