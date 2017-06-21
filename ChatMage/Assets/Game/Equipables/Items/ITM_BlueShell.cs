using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

public class ITM_BlueShell : Item
{
    public bool enable = false; // A ENLEVER

    public GameObject blueShellPrefab;

    public float shellSpeed = 1;

    public float cooldown;
    private float countdown;

    [InspectorRange(1, 10)]
    public float loopIntensity = 1;
    [InspectorRange(1, 10)]
    public float noiseSpeed = 1;
    public float wonderCooldown = 2;

    private bool shellSpawned = false;

    public override void OnGameReady()
    {
        countdown = cooldown;
    }

    public override void OnGameStarted()
    {
        shellSpawned = false;

        enable = false; // A ENLEVER
    }

    public override void OnUpdate()
    {
        if (!enable)  // A ENLEVER
            return;
        if (countdown < 0)
        {
            if (shellSpawned)
                return;
            LaunchShell();
        }
        countdown -= Time.deltaTime;
    }

    void LaunchShell()
    {   
        GameObject shell = Instantiate(blueShellPrefab);
        shellSpawned = true;

        shell.GetComponent<BlueShellScript>().SetValues(shellSpeed, noiseSpeed, wonderCooldown, loopIntensity);
        shell.GetComponent<BlueShellScript>().onHit += delegate () { shellSpawned = false; };

        countdown = cooldown;
    }
}
