using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Shield : NetworkBehaviour
{
    [SyncVar(hook = "OnShieldEnabledChange")]
    public bool shieldEnabled = false;

    public float durationShield = 2;
    float timeStartShield = 0;

    public GameObject shieldGO = null;

    public static float Cooldown = 15.0f;

    public bool ShieldReady()
    {
        return timeStartShield == 0 || Time.time - timeStartShield > Cooldown;
    }

    void OnShieldEnabledChange(bool newShieldEnabled)
    {
        shieldEnabled = newShieldEnabled;

        if(shieldEnabled)
            EnableShield();
        else DisableShield();
    }

    public void EnableShield()
    {
        shieldEnabled = true;
        timeStartShield = Time.time;
        if(shieldGO != null)
            shieldGO.SetActive(true);
    }

    void DisableShield()
    {
        shieldEnabled = false;
        if(shieldGO != null)
            shieldGO.SetActive(false);
    }

    void Update()
    {
        if(shieldEnabled && Time.time - timeStartShield > durationShield)
            DisableShield();
    }
}
