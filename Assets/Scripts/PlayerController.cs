using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField]
	float speed = 1.0f;
	
	new Rigidbody2D rigidbody;

	void Start ()
	{
		rigidbody = GetComponent<Rigidbody2D>();
	}
	
	void Update ()
	{
		Move(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
	}

	void Move(Vector2 direction)
	{
		const float factor = 5.0f;
		/*transform.position = new Vector3(transform.position.x + direction.x * speed * factor, 
										transform.position.y + direction.y * speed * factor, 
										transform.position.z);*/
		rigidbody.velocity = new Vector3(direction.x * speed * factor, direction.y * speed * factor, 0);
	}
}
