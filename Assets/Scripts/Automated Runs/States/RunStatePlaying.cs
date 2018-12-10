using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunStatePlaying : RunState
{
    public RunStatePlaying(): base()
    {
        timeUntilTransition = 30;

        Player player = GameObject.FindObjectOfType<GameController>().player;
        if(player.GetComponent<AI>() == null)
            player.gameObject.AddComponent<AI>();
    }

    public override RunState Transite()
    {
        GameObject.FindObjectOfType<GameController>().LeaveGame(false);
        
		Scene scene = SceneManager.GetActiveScene();
		SceneManager.LoadScene(scene.name);

        return new RunStateIdle();
    }

    public override string Name()
    {
        return "Playing";
    }
}
