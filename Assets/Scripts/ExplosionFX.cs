using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionFX : MonoBehaviour
{
	void Start ()
	{
		Destroy(gameObject, 0.5f);
	}
}
