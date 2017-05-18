using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_DemoLazer : Item
{
    public GameObject lazerVisuals;

    public override void Init(PlayerController player)
    {
        base.Init(player);

        Debug.Log("Laser placed ! (not yet)");
    }

    public override void OnGameReady()
    {

    }

    public override void OnGameStarted()
    {

    }

    public override void OnUpdate()
    {

    }
}
