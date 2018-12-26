using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeRemaining : MonoBehaviour
{
    Text text;

    int timeToQuit = 0;
    public static float timeStart = 0;

    static float lastTextUpdate = 0;
    static float timeToUpdate = 20;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("timeToQuit") == 0)
        {
            Destroy(gameObject);
            return;
        }
        timeToQuit = PlayerPrefs.GetInt("timeToQuit");
        text = GetComponent<Text>();


        if(timeStart == 0)
            timeStart = Time.time;
        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastTextUpdate > timeToUpdate)
            UpdateText();
    }

    void UpdateText()
    {
        lastTextUpdate = Time.time;
        text.text = "Remaining: " + Mathf.Round(timeToQuit - (Time.time - timeStart) / 60.0f) + " mins";
    }
}
