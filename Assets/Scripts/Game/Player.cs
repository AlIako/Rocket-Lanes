using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
	[SerializeField]
	private int id = 0;
	public int Id { get { return id; } }

	[HideInInspector]
	public int neighbourPlayerId;

	[SerializeField]
	private int health = 10;
	public int Health { get { return health; } }

	private Color color = new Color(1.0f, 1.0f, 1.0f);
	public Color Color { get { return color; } }

	void Start()
	{
		ComputeNeighbourId();
	}

	//server-client only
	public override void OnStartAuthority()
	{
		GameController gc = GameObject.FindObjectOfType<GameController>();
		gc.player = this;

		PlayerController pc = GetComponent<PlayerController>();
		pc.enabled = true;
	}

	void ComputeNeighbourId()
	{
		neighbourPlayerId = -1;
		if(Id != -1)
		{
			neighbourPlayerId = Id + 1;
			if(neighbourPlayerId >= 4)
				neighbourPlayerId = 0;
		}
	}

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

	public void PickColor()
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

	public void ApplyColor() { ApplyColor(color); }
	public void ApplyColor(Color color)
	{
		this.color = color;
		GetComponent<SpriteRenderer>().color = color;
	}
}
