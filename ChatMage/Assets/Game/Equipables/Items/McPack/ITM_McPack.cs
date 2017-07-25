using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_McPack : Item {

    [FullInspector.InspectorHeader("Health packs")]
    public float appearanceMultiplier = 0.7f;
    public int regenAmount = 2;

    public override void OnGameReady()
    {
    }

    public override void OnGameStarted()
    {
        HealthPackManager hpMan = Game.instance.healthPackManager;

        hpMan.luckMultiplier = appearanceMultiplier;
        hpMan.healthPackPrefab.regenAmount = regenAmount;
    }

    public override void OnUpdate()
    {
    }
}
