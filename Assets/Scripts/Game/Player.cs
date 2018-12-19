using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
	public Lane lane;

	[SerializeField, SyncVar(hook = "SetHealth")]
	private int health = 10;
	public int Health { get { return health; } }

	bool alive = true;
	Shield shield = null;
	
	void Start()
	{
		shield = GetComponent<Shield>();
	}

	public bool ShieldEnabled()
	{
		return shield.shieldEnabled;
	}

	public void CastShield()
	{
		shield.EnableShield();
	}

	public void LoseHealth(int value)
	{
		health -= value;

		if(alive && health <= 0)
			Die();
		else if(!alive && health > 0)
			Rez();
	}

	public void SetHealth(int value)
	{
		health = value;

		if(alive && health <= 0)
			Die();
		else if(!alive && health > 0)
			Rez();
	}

	void Die()
	{
		health = 0;
		alive = false;
		((SpriteRenderer)GetComponent<SpriteRenderer>()).enabled = false;
	}

	void Rez()
	{
		alive = true;
		((SpriteRenderer)GetComponent<SpriteRenderer>()).enabled = true;
	}

	public void ApplyColor(Color color)
	{
		GetComponent<SpriteRenderer>().color = color;
	}


	//server-client only
	public override void OnStartAuthority()
	{
		GameController gc = GameObject.FindObjectOfType<GameController>();
		gc.player = this;

		PlayerController pc = GetComponent<PlayerController>();
		pc.enabled = true;
	}

	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Lane>())
        {
            other.GetComponent<Lane>().Enter(this);
        }
    }

	void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Lane>())
        {
            other.GetComponent<Lane>().Leave(this);
        }
    }
}
