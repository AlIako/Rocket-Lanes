using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunStateSCPlaying : RunState
{
    int timeUntilStopPlaying = 0;
    int cdCheckTimeout = 3;

    public RunStateSCPlaying(): base()
    {
        timeUntilStopPlaying = Random.Range(20, 40);
        timeUntilTransition = cdCheckTimeout; //timeout checker

        Player player = GameObject.FindObjectOfType<GameController>().player;
        if(player.GetComponent<AI>() == null)
            player.gameObject.AddComponent<AI>();
    }

    public override RunState Transite()
    {
        if(Time.time - timeStartState > timeUntilStopPlaying)
        {
            if(AutomatedServerClientRunController.status == 0)
            {
                // host resets his pv
                GameObject.FindObjectOfType<GameController>().player.SetHealth(5);
                return this;
            }
            else // client leaves game
            {
                GameObject.FindObjectOfType<GameController>().LeaveGame(false);
                
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);

                return new RunStateSCIdle();
            }
        }
        else
        {
            timeUntilTransition += cdCheckTimeout;
            
            //only check time out
            bool timeOut = GameObject.FindObjectOfType<GameController>().player == null;
            if(timeOut)
            {
                GameObject.FindObjectOfType<GameController>().LeaveGame(false);
                
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);

                return new RunStateSCIdle();
            }
            return this;
        }
    }

    public override string Name()
    {
        return "Playing";
    }
}
