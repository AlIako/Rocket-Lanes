using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
	public void Click()
	{
		GameController gc = GameObject.FindObjectOfType<GameController>();
		gc.LeaveGame(true);
		//SceneManager.LoadScene("Main Menu");
	}

	public void ReloadCurrent()
	{
		Scene scene = SceneManager.GetActiveScene(); 
		SceneManager.LoadScene(scene.name);
	}
}
