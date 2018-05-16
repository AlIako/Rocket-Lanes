using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
	Player player;
	Rigidbody2D rb;
	float speed = 1.0f;

	float timeBetweenDirections = 2.0f;
	float nextTimeBetweenDirections = 2.0f;
	float lastDirection = 0.0f;
	float randomXDirection = 0.0f;
	float randomYDirection = 0.0f;
	
	void Start ()
	{
		rb = GetComponent<Rigidbody2D>();
		lastDirection = Time.time;
	}
	
	void Update ()
	{
		PickDirection();
		Move(new Vector2(randomXDirection, randomYDirection));
	}

	void PickDirection()
	{
		if(Time.time - lastDirection > nextTimeBetweenDirections)
		{
			randomXDirection = Random.Range(-1.0f, 1.0f);
			randomYDirection = Random.Range(-1.0f, 1.0f);

			nextTimeBetweenDirections = timeBetweenDirections + 
										Random.Range(-timeBetweenDirections/10.0f, timeBetweenDirections/10.0f);
			
			lastDirection = Time.time;
		}
	}

	void Move(Vector2 direction)
	{
		direction.Normalize();
		rb.velocity = new Vector3(direction.x * speed, direction.y * speed, 0);
	}
}
