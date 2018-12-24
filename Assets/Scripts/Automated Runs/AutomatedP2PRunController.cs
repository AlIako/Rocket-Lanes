using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//DEA
public class AutomatedP2PRunController : MonoBehaviour
{
    RunState currentState;
    
    public static GameController gameController = null;
    public static P2PController p2PController = null;

    public static int myPort = 0;
    public static List<int> targetPorts = null;
    public static int range = 5;

    Text stateText;

	static float timeStartGame = 0;
	static float timeQuitGame = 0;

    void Start()
    {
        gameController = GameObject.FindObjectOfType<GameController>();
        p2PController = GameObject.FindObjectOfType<P2PController>();
        stateText = GameObject.FindGameObjectWithTag("State").GetComponent<Text>();

        Text statusText = GameObject.FindGameObjectWithTag("Status").GetComponent<Text>();
        statusText.text = "Port: " + myPort;

        IniTargetPorts();

        currentState = new RunStateIdle();
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

    public static void SelectPort(int value)
    {
        PlayerPrefs.SetInt("myPortIndex", value);
        
        IniTargetPorts();
        myPort = targetPorts[value];
    }

    static void IniTargetPorts()
    {
        if(targetPorts == null)
        {
            targetPorts = new List<int>();
            targetPorts.Add(8881);
            targetPorts.Add(8882);
            targetPorts.Add(8883);
            targetPorts.Add(8884);
            targetPorts.Add(8885);
            targetPorts.Add(8886);
            targetPorts.Add(8887);
            targetPorts.Add(8888);
            targetPorts.Add(8889);
            targetPorts.Add(8890);
        }
    }

    public static void UpdateRange(int range)
    {
        PlayerPrefs.SetInt("range", range);
        AutomatedP2PRunController.range = range + 1;
    }
}
