using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_McPack : Item {

    public float maxChance = 35;
    public int regenAmount = 2;

    public override void OnGameReady()
    {
    }

    public override void OnGameStarted()
    {
        Game.instance.healthPackManager.lerpCeiling = maxChance;
        Game.instance.healthPackManager.healthPackPrefab.regenAmount = regenAmount;
    }

    public override void OnUpdate()
    {
    }
}
