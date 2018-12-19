using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
	public GameObject choseP2P;
	public GameObject errorText;
	public GameObject errorPanel;

	[SerializeField]
	GameObject inGameUI;

	GameController gameController;
	List<GameObject> laneUIs = null;
	List<GameObject> laneButtonUIs;

	void Start()
	{
		gameController = FindObjectOfType<GameController>();
	}
	
	public void DisplayError(string er)
	{
		errorText.GetComponent<Text>().text = er;
		errorPanel.SetActive(true);
	}

	public void EnterGameUI()
	{
		if(choseP2P)
			choseP2P.SetActive(false);
		
		inGameUI.SetActive(true);
		
		if(laneUIs == null)
		{
			laneUIs = new List<GameObject>();
			laneButtonUIs = new List<GameObject>();
			laneUIs.Add(GameObject.FindGameObjectWithTag("Lane1UI"));
			laneUIs.Add(GameObject.FindGameObjectWithTag("Lane2UI"));
			laneUIs.Add(GameObject.FindGameObjectWithTag("Lane3UI"));
			laneUIs.Add(GameObject.FindGameObjectWithTag("Lane4UI"));
			foreach(GameObject laneUI in laneUIs)
			{
				GameObject button = laneUI.GetComponentInChildren<ButtonContainer>().gameObject;
				laneButtonUIs.Add(button);
				button.SetActive(false);
				laneUI.SetActive(false);
			}
		}
	}

	public void ActivateLaneUI(int index)
	{
		laneUIs[index].SetActive(true);
		if(gameController.lanes[index].player == gameController.player)
		{
			laneButtonUIs[index].SetActive(true);
		}
	}

	public void DeactivateLaneUI(int index)
	{
		laneUIs[index].SetActive(false);
	}

	public void UpdateGameColor(Color color)
	{
		GameObject.FindGameObjectWithTag("GameColor").GetComponent<Image>().color = color;
	}
}
