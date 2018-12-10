using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunStateIdle : RunState
{
    public RunStateIdle(): base()
    {
        timeUntilTransition = Random.Range(2, 4);
    }

    public override RunState Transite()
    {
        //pick a port
        int targetPortId = Random.Range(0, AutomatedP2PRunController.targetPorts.Count);

        AutomatedP2PRunController.p2PController.myPort = AutomatedP2PRunController.myPort;
        int targetPort = AutomatedP2PRunController.targetPorts[targetPortId];

        if(targetPort == AutomatedP2PRunController.myPort)
        {
            AutomatedP2PRunController.p2PController.NewGame();
            return new RunStateJoining();
        }
        else
        {
            AutomatedP2PRunController.p2PController.SetTargetPort(targetPort);
            AutomatedP2PRunController.p2PController.JoinGame();
            return new RunStateJoining(targetPort);
        }

    }
    
    public override string Name()
    {
        return "Idle";
    }
}
