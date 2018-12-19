using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldButton : MonoBehaviour
{
    public float cooldown = 15;
    bool onCooldown = false;
    float clickedAt = 0;

    public void Click()
    {
        onCooldown = true;
        clickedAt = Time.time;

        GetComponent<Button>().interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(onCooldown && Time.time - clickedAt > cooldown)
        {
            onCooldown = false;
            GetComponent<Button>().interactable = true;
        }
    }
}
