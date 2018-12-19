using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
	Player player;
	Rigidbody2D rb;
	float speed = 1.0f;
	GameController gameController;

	float timeBetweenDirections = 2.0f;
	float nextTimeBetweenDirections = 2.0f;
	float lastDirection = 0.0f;
	float randomXDirection = 0.0f;
	float randomYDirection = 0.0f;

	float timeBetweenSendRockets = 1.0f;
	float nextTimeBetweenSendRockets = 1.0f;
	float lastSendRocket = 0.0f;

	float timeBetweenUseShield = 15.0f;
	float lastUseShield = 0.0f;
	
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		player = GetComponent<Player>();
		gameController = GameObject.FindObjectOfType<GameController>();
		lastDirection = Time.time;
		lastSendRocket = Time.time;
		lastUseShield = Time.time - timeBetweenUseShield / 2;
	}
	
	void Update()
	{
		PickDirection();
		SendRocket();
		UseShield();
		Move(new Vector2(randomXDirection, randomYDirection));
	}

	void SendRocket()
	{
		if(Time.time - lastSendRocket > nextTimeBetweenSendRockets)
		{
			gameController.SendRocket(player.lane.id, gameController.GetNextAliveLaneId(player));
			nextTimeBetweenSendRockets = timeBetweenSendRockets + 
										Random.Range(-timeBetweenSendRockets/10.0f, timeBetweenSendRockets/10.0f);
			
			lastSendRocket = Time.time;
		}
	}

	void UseShield()
	{
		if(Time.time - lastUseShield > timeBetweenUseShield)
		{
			gameController.CastShield(player.lane.id);
			lastUseShield = Time.time;
		}
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
