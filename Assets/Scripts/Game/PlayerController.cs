using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField]
	float speed = 1.0f;
	
	Rigidbody2D rb;

	void Start ()
	{
		rb = GetComponent<Rigidbody2D>();
	}
	
	void Update ()
	{
		Move(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
	}
	
	void Move(Vector2 direction)
	{
		rb.velocity = new Vector3(direction.x * speed, direction.y * speed, 0);
	}
}
