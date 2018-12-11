using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    void Start()
    {
        Dropdown dropdown = GameObject.FindGameObjectWithTag("DropdownP2P").GetComponent<Dropdown>();
        dropdown.value = PlayerPrefs.GetInt("myPortIndex", 0);
        AutomatedP2PRunController.SelectPort(dropdown.value);
        
        Dropdown dropdown2 = GameObject.FindGameObjectWithTag("DropdownServerClient").GetComponent<Dropdown>();
        dropdown2.value = PlayerPrefs.GetInt("myStatusSC", 0);
        AutomatedServerClientRunController.SelectStatus(dropdown2.value);
        
        Dropdown dropdown3 = GameObject.FindGameObjectWithTag("DropdownP2PRange").GetComponent<Dropdown>();
        dropdown3.value = PlayerPrefs.GetInt("range", 4);
        AutomatedP2PRunController.UpdateRange(dropdown3.value);
    }


    public void StartLevel(string level)
    {
        SceneManager.LoadScene(level);
    }

    public void UpdateDropdownP2PA()
    {
        Dropdown dropdown = GameObject.FindGameObjectWithTag("DropdownP2P").GetComponent<Dropdown>();
        AutomatedP2PRunController.SelectPort(dropdown.value);
    }

    public void UpdateDropdownP2PARange()
    {
        Dropdown dropdown = GameObject.FindGameObjectWithTag("DropdownP2PRange").GetComponent<Dropdown>();
        AutomatedP2PRunController.UpdateRange(dropdown.value);
    }

    public void UpdateDropdownServerClientA()
    {
        Dropdown dropdown = GameObject.FindGameObjectWithTag("DropdownServerClient").GetComponent<Dropdown>();
        AutomatedServerClientRunController.SelectStatus(dropdown.value);
    }
}
