using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunStateJoining : RunState
{
    int targetPort = 0;

    public RunStateJoining(): base()
    {
        timeUntilTransition = 1;
    }

    public RunStateJoining(int targetPort): base()
    {
        this.targetPort = targetPort;
        timeUntilTransition = 1;
    }

    public override RunState Transite()
    {
        if(GameController.gameStarted)
        {
            return new RunStatePlaying();
        }
        else if(GameObject.FindGameObjectWithTag("ErrorPanel") != null)
        {
            return new RunStateInError();
        }

        return new RunStateJoining(targetPort);
    }
    
    public override string Name()
    {
        if(targetPort == 0)
            return "Creating";
        else return "Joining " + targetPort;
    }
}
