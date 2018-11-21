using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour
{
	public GameObject choseP2P;
	public GameObject error;
	public GameController gameController;

	public void Click()
	{
		choseP2P.SetActive(true);
		error.SetActive(false);
	}
}
