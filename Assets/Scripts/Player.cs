using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField]
	private int id = 0;
	public int Id { get { return id; } }

	[SerializeField]
	private int health = 10;
	public int Health { get { return health; } }

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
		ApplyColor();
	}

	void ApplyColor()
	{
		if(id == 0)
		{
			GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f);
		}
		else if(id == 1)
		{
			GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.3f, 1.0f);
		}
		else if(id == 2)
		{
			GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.85f, 0.0f);
		}
		else if(id == 3)
		{
			GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 0.0f);
		}
	}
}
