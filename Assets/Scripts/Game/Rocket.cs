using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
	[SerializeField]
	float speed = 1.0f;
	[SerializeField]
	GameObject explosionFX;

	Lane lane = null;
	GameController gameController = null;

	void Start()
	{
		GetComponent<Rigidbody2D>().velocity = new Vector3(0, -speed, 0);
		gameController = GameObject.FindObjectOfType<GameController>();
	}

	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Lane>())
            lane = other.GetComponent<Lane>();
		else if(gameController.HandleCollisions(lane))
		{
			Player player = other.GetComponent<Player>();
			if (player != null)
			{
				if(!player.ShieldEnabled())
					player.LoseHealth(1);
				Destroy(gameObject);
			}
			else if (other.tag.Equals("Bottom"))
			{
				Destroy(gameObject);
			}
		}
    }

	public void OnDestroy()
    {
		if(GameController.gameStarted)
			Instantiate(explosionFX, transform.position, Quaternion.identity);
    }
}
