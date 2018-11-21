using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameButton : MonoBehaviour
{
	public void Click()
	{
		P2PController c = FindObjectOfType<P2PController>();
		c.NewGame();
	}
}
