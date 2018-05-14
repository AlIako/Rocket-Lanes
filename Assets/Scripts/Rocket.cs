using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
	[SerializeField]
	float speed = 1.0f;

	void Start()
	{
		GetComponent<Rigidbody2D>().velocity = new Vector3(0, -speed, 0);
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		GameObject go = collision.gameObject;

		Player player = go.GetComponent<Player> ();
		if (player != null)
		{
			player.LoseHealth (1);
			Destroy(gameObject);
		}
		else if (go.GetComponent<Bottom>() != null)
		{
			Destroy(gameObject);
		}
	}
}
