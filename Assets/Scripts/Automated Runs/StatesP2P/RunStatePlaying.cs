using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunStatePlaying : RunState
{
    public RunStatePlaying(): base()
    {
        timeUntilTransition = Random.Range(20, 40);

        Player player = GameObject.FindObjectOfType<GameController>().player;
        if(player != null && player.GetComponent<AI>() == null)
            player.gameObject.AddComponent<AI>();
        
        if(player == null)
            Debug.Log(AutomatedP2PRunController.myPort + " Error: gameController.player is null");
    }

    public override RunState Transite()
    {
        if(PlayerPrefs.GetInt("CreatorNeverLeaves") == 1)
        {
            if(AutomatedP2PRunController.lastTargetPort == AutomatedP2PRunController.myPort)
                return this;
        }

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
