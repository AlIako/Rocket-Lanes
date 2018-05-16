using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
	[SerializeField]
	private int id = 0;
	public int Id { get { return id; } }

	[SerializeField]
	private int health = 10;
	public int Health { get { return health; } }

	[SyncVar(hook = "OnChangeColor")]
	Color color;

	public void LoseHealth(int value)
	{
		health -= value;

		if(health <= 0)
		{
			Destroy(gameObject);
		}
	}

	public void SetId(int id)
	{
		this.id = id;
	}
	
	void OnChangeColor(Color color)
    {
		GetComponent<SpriteRenderer>().color = color;
    }
	public override void OnStartClient()
	{
		//https://answers.unity.com/questions/1143773/syncvar-synchronizes-correctly-but-not-the-text-co.html
		// Call it directly to update color component on other clients who join the game later.
		OnChangeColor(color);
	}

	public override void OnStartAuthority()
	{
		pickColor();
	}

	void pickColor()
	{
		if(id == 0)
			color = new Color(1.0f, 0.0f, 0.0f);
		else if(id == 1)
			color = new Color(0.0f, 0.3f, 1.0f);
		else if(id == 2)
			color = new Color(0.3f, 0.85f, 0.0f);
		else if(id == 3)
			color = new Color(1.0f, 1.0f, 0.0f);
	}
}
