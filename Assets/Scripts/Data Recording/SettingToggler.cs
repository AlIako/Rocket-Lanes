using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingToggler : MonoBehaviour
{
    Toggle toggle;

    void Start()
    {
        toggle = GetComponent<Toggle>();
    	toggle.isOn = PlayerPrefs.GetInt("Recording", 0) == 1;
    }

    public void Toggle()
    {
        PlayerPrefs.SetInt("Recording", Convert.ToInt32(toggle.isOn));
    }
}
