using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunStateSCIdle : RunState
{
    public RunStateSCIdle(): base()
    {
        timeUntilTransition = Random.Range(2, 4);
    }

    public override RunState Transite()
    {
        Debug.Log("Joining...");
        if(AutomatedServerClientRunController.status == 0)
        {
            AutomatedServerClientRunController.serverClientController.StartHost();
        }
        else
        {
            AutomatedServerClientRunController.serverClientController.StartClient();
        }
        return new RunStateSCJoining();

    }
    
    public override string Name()
    {
        return "Idle";
    }
}
