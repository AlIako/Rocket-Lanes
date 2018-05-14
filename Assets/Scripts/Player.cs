using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
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
}
