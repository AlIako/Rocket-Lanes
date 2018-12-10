using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    void Start()
    {
        Dropdown dropdown = GameObject.FindObjectOfType<Dropdown>();
        dropdown.value = PlayerPrefs.GetInt("myPortIndex", 0);
        AutomatedP2PRunController.SelectPort(dropdown.value);
    }


    public void StartLevel(string level)
    {
        SceneManager.LoadScene(level);
    }

    public void UpdateDropdownP2PA()
    {
        Dropdown dropdown = GameObject.FindObjectOfType<Dropdown>();
        AutomatedP2PRunController.SelectPort(dropdown.value);
    }
}
