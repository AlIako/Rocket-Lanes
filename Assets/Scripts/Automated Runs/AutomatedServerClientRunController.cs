using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AutomatedServerClientRunController : MonoBehaviour
{
    RunState currentState;
    
    public static GameController gameController = null;
    public static MyNetworkManager serverClientController = null;
    public static int status = 0; //0 = hosting, 1 = joining

    Text stateText;

	static float timeStartGame = 0;
	static float timeQuitGame = 0;

    void Start()
    {
        gameController = GameObject.FindObjectOfType<GameController>();
        serverClientController = GameObject.FindObjectOfType<MyNetworkManager>();
        stateText = GameObject.FindGameObjectWithTag("State").GetComponent<Text>();
        
        Text statusText = GameObject.FindGameObjectWithTag("Status").GetComponent<Text>();
        if(status == 0)
            statusText.text = "Server";
        else statusText.text = "Client";

        currentState = new RunStateSCIdle();
    }

    void Update()
    {
        if(currentState.ReadyToTransite())
        {
            currentState = currentState.Transite();
            stateText.text = "State: " + currentState.Name();
        }

		if(timeStartGame != 0)
		{
			if(Time.time - timeQuitGame > 0)
			{
                if(GameController.gameStarted)
    				gameController.LeaveGame(true);
                else
                    SceneManager.LoadScene("Main Menu");
			}
		}
    }
    
    public static void ResetQuitTimers()
    {
        timeStartGame = 0;
        timeQuitGame = 0;

		if(PlayerPrefs.GetInt("timeToQuit") != 0)
		{
			timeStartGame = Time.time;
			timeQuitGame = timeStartGame + PlayerPrefs.GetInt("timeToQuit") * 60.0f;
		}
    }

    public static void SelectStatus(int value)
    {
        PlayerPrefs.SetInt("myStatusSC", value);
        status = value;
    }
}
