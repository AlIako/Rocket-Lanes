using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyPortField : MonoBehaviour
{
	void Start()
	{
		UpdateField();
	}
	
	public void UpdateField()
	{
		P2PController c = GameObject.FindObjectOfType<P2PController>();
		c.SetMyPort(Int32.Parse(GetComponent<InputField>().text));
	}
}
