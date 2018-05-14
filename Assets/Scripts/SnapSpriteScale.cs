using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapSpriteScale : MonoBehaviour
{
	[SerializeField]
	float snap = .32f;

	public void Snap()
	{
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.size = new Vector2(SnapFloat(spriteRenderer.size.x), SnapFloat(spriteRenderer.size.y));
	}

	float SnapFloat(float value)
	{
		return Mathf.Round(value / snap) * snap;
	}
}