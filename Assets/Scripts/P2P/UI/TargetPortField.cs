using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetPortField : MonoBehaviour
{
	void OnEnable()
	{
		UpdateField();
	}
	
	public void UpdateField()
	{
		P2PController c = GameObject.FindObjectOfType<P2PController>();
		c.SetTargetPort(Int32.Parse(GetComponent<InputField>().text));
	}
}
