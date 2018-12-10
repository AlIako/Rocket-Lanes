using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunStateInError : RunState
{
    public RunStateInError(): base()
    {
        timeUntilTransition = 2;
    }

    public override RunState Transite()
    {
        BackButton button = GameObject.FindObjectOfType<BackButton>();
        button.ReloadCurrent();
        return new RunStateIdle();
    }
    
    public override string Name()
    {
        return "In Error";
    }
}
