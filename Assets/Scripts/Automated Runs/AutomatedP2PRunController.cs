using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//DEA
public class AutomatedP2PRunController : MonoBehaviour
{
    [SerializeField]
    RunState currentState;
    
    public static GameController gameController = null;
    public static P2PController p2PController = null;

    public static int myPort = 0;
    public static List<int> targetPorts = null;

    Text stateText;

    void Start()
    {
        gameController = GameObject.FindObjectOfType<GameController>();
        p2PController = GameObject.FindObjectOfType<P2PController>();
        stateText = GameObject.FindGameObjectWithTag("State").GetComponent<Text>();

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
        }
    }
}
