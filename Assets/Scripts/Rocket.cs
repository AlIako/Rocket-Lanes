using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
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
