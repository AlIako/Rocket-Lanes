using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState
{
    protected float timeStartState = 0;
    protected int timeUntilTransition = 1;

    public RunState()
    {
        timeStartState = Time.time;
        Debug.Log("Entered State " + Name());
    }

    virtual public bool ReadyToTransite()
    {
        return Time.time - timeStartState > timeUntilTransition;
    }

    virtual public RunState Transite()
    {
        return null;
    }

    virtual public string Name()
    {
        return "Noname";
    }
}
