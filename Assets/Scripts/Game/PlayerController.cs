using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField]
	float speed = 1.0f;
	
	Rigidbody2D rb;
	Player player;

	void Start ()
	{
		rb = GetComponent<Rigidbody2D>();
		player = GetComponent<Player>();
	}
	
	void Update ()
	{
		Move(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
	}

	void FixedUpdate()
	{
		SteadyRotation();
	}

	void Move(Vector2 direction)
	{
		rb.velocity = new Vector3(direction.x * speed, direction.y * speed, 0);
	}

	void SteadyRotation()
	{
		rb.angularVelocity = 0;
		transform.rotation = Quaternion.identity;
	}
}
