using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerNetwork : NetworkBehaviour
{
	Player player;

	[SyncVar(hook = "OnChangeColor")]
	Color color;

	void Start()
	{
		player = GetComponent<Player>();
	}

	public void SetColor(Color color)
	{
		this.color = color;
	}
	
	void OnChangeColor(Color color)
    {
		player.ApplyColor(color);
    }

	public override void OnStartClient()
	{
		//https://answers.unity.com/questions/1143773/syncvar-synchronizes-correctly-but-not-the-text-co.html
		// Call it directly to update color component on other clients who join the game later.
		OnChangeColor(color);
	}

	public override void OnStartAuthority()
	{
		player.PickColor();
	}

}
