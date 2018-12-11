using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutomatedServerClientRunController : MonoBehaviour
{
    RunState currentState;
    
    public static GameController gameController = null;
    public static MyNetworkManager serverClientController = null;
    public static int status = 0; //0 = hosting, 1 = joining

    Text stateText;

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
    }

    public static void SelectStatus(int value)
    {
        PlayerPrefs.SetInt("myStatusSC", value);
        status = value;
    }
}
