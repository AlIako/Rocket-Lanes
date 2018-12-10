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
        
        if(timeUntilTransition >= 2) //timeout after 2 sec
        {
            GameObject.FindObjectOfType<GameController>().LeaveGame(false);
            
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        timeUntilTransition *= 2;
        return this;
    }
    
    public override string Name()
    {
        if(AutomatedServerClientRunController.status == 0)
            return "Hosting";
        return "Joining";
    }
}
