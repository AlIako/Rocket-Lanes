using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    void Start()
    {
        TimeRemaining.timeStart = 0;
        
        Dropdown dropdown = GameObject.FindGameObjectWithTag("DropdownP2P").GetComponent<Dropdown>();
        dropdown.value = PlayerPrefs.GetInt("myPortIndex", 0);
        AutomatedP2PRunController.SelectPort(dropdown.value);
        
        Dropdown dropdown2 = GameObject.FindGameObjectWithTag("DropdownServerClient").GetComponent<Dropdown>();
        dropdown2.value = PlayerPrefs.GetInt("myStatusSC", 0);
        AutomatedServerClientRunController.SelectStatus(dropdown2.value);
        
        Dropdown dropdown3 = GameObject.FindGameObjectWithTag("DropdownP2PRange").GetComponent<Dropdown>();
        dropdown3.value = PlayerPrefs.GetInt("range", 4);
        AutomatedP2PRunController.UpdateRange(dropdown3.value);
        
        InputField timeToQuitField = GameObject.FindGameObjectWithTag("TimeToQuitField").GetComponent<InputField>();
        timeToQuitField.text = PlayerPrefs.GetInt("timeToQuit", 0).ToString();
        
        InputField subfolderField = GameObject.FindGameObjectWithTag("SubfolderField").GetComponent<InputField>();
        subfolderField.text = PlayerPrefs.GetString("subfolder", "");
        
        Toggle creatorNeverLeaves = GameObject.FindGameObjectWithTag("CreatorNeverLeaves").GetComponent<Toggle>();
        creatorNeverLeaves.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("CreatorNeverLeaves", 0));
    }


    public void StartLevel(string level)
    {
        SceneManager.LoadScene(level);
        AutomatedP2PRunController.ResetQuitTimers();
        AutomatedServerClientRunController.ResetQuitTimers();
    }

    public void UpdateCreatorNeverLeaves()
    {
        Toggle creatorNeverLeaves = GameObject.FindGameObjectWithTag("CreatorNeverLeaves").GetComponent<Toggle>();
        PlayerPrefs.SetInt("CreatorNeverLeaves", Convert.ToInt32(creatorNeverLeaves.isOn));
    }

    public void UpdateSubfolder()
    {
        InputField subfolderField = GameObject.FindGameObjectWithTag("SubfolderField").GetComponent<InputField>();
        PlayerPrefs.SetString("subfolder", subfolderField.text);
    }

    public void UpdateTimeToQuit()
    {
        InputField timeToQuitField = GameObject.FindGameObjectWithTag("TimeToQuitField").GetComponent<InputField>();
        PlayerPrefs.SetInt("timeToQuit", Convert.ToInt32(timeToQuitField.text));
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
