using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerNetwork : NetworkBehaviour
{
	//THIS CLASS IS DEPRECATED FOR NOW
	
	[SyncVar(hook = "OnChangeColor")]
	Color color;

	public void SetColor(Color color)
	{
		this.color = color;
	}
	
	void OnChangeColor(Color color)
    {
		//GetComponent<Player>().ApplyColor(color);
    }

	public override void OnStartClient()
	{
		//https://answers.unity.com/questions/1143773/syncvar-synchronizes-correctly-but-not-the-text-co.html
		// Call it directly to update color component on other clients who join the game later.
		//OnChangeColor(color);
		GetComponent<Player>().ApplyColor(color);
	}

	public override void OnStartAuthority()
	{
		//GetComponent<Player>().PickColor();
	}

}
