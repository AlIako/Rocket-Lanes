using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
	public void Click()
	{
		SceneManager.LoadScene("Main Menu");
	}
}
