using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
	public GameObject choseP2P;
	public GameObject errorText;
	public GameObject errorPanel;
	
	public void DisplayError(string er)
	{
		errorText.GetComponent<Text>().text = er;
		errorPanel.SetActive(true);
	}
}
