using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetIp : MonoBehaviour
{
	void OnEnable()
	{
		UpdateField();
	}
	
	public void UpdateField()
	{
		P2PController c = GameObject.FindObjectOfType<P2PController>();

		if(c != null)
			c.SetTargetIp(GetComponent<InputField>().text);

		MyNetworkManager m = GameObject.FindObjectOfType<MyNetworkManager>();
		if(m != null)
			m.targetIp = GetComponent<InputField>().text;
	}
}
