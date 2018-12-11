using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunStateSCJoining : RunState
{

    public RunStateSCJoining(): base()
    {
        timeUntilTransition = 1;
    }

    public override RunState Transite()
    {
        if(GameController.gameStarted)
        {
            return new RunStateSCPlaying();
        }
        else if(GameObject.FindGameObjectWithTag("ErrorPanel") != null)
        {
            return new RunStateInError();
        }

        return new RunStateSCJoining();
    }
    
    public override string Name()
    {
        if(AutomatedServerClientRunController.status == 0)
            return "Hosting";
        return "Joining";
    }
}
