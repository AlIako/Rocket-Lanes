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
        int targetPortId = Random.Range(0, AutomatedP2PRunController.range);

        AutomatedP2PRunController.p2PController.myPort = AutomatedP2PRunController.myPort;
        AutomatedP2PRunController.lastTargetPort = AutomatedP2PRunController.targetPorts[targetPortId];

        if(AutomatedP2PRunController.lastTargetPort == AutomatedP2PRunController.myPort)
        {
            AutomatedP2PRunController.p2PController.NewGame();
            return new RunStateJoining();
        }
        else
        {
            AutomatedP2PRunController.p2PController.SetTargetPort(AutomatedP2PRunController.lastTargetPort);
            AutomatedP2PRunController.p2PController.JoinGame();
            return new RunStateJoining(AutomatedP2PRunController.lastTargetPort);
        }

    }
    
    public override string Name()
    {
        return "Idle";
    }
}
