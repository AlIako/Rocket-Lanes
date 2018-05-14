using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public int id = 0;
	public int health = 5;

	void Start ()
	{
		
	}
	
	void Update ()
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
}
