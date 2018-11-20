using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetIp : MonoBehaviour
{
	void Start()
	{
		UpdateField();
	}
	
	public void UpdateField()
	{
		P2PController c = GameObject.FindObjectOfType<P2PController>();
		c.SetTargetIp(GetComponent<InputField>().text);
	}
}
