using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
	public Lane lane;

	[SerializeField]
	private int health = 10;
	public int Health { get { return health; } }

	void Start()
	{
	}

	public void LoseHealth(int value)
	{
		health -= value;

		if(health <= 0)
		{
			Destroy(gameObject);
		}
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
