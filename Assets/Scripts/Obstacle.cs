using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
	public void AdaptColliderToSprite()
	{
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
		BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();

		boxCollider.size = new Vector2(spriteRenderer.size.x, spriteRenderer.size.y);
	}
}