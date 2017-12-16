using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

public class ITM_McPack : Item
{

    [InspectorHeader("Health packs"), InspectorRange(0, 1)]
    public float appearanceMultiplier = 0.7f;
    public int regenAmount = 2;

    public override void Equip(int duplicateIndex)
    {
        throw new NotImplementedException();
    }

    public override void OnGameReady()
    {
    }

    public override void OnGameStarted()
    {
        HealthPackManager hpMan = Game.instance.healthPackManager;

        hpMan.luckMultiplier *= appearanceMultiplier;
        hpMan.healthPackValue = regenAmount;
    }

    public override void OnUpdate()
    {
    }

    public override void Unequip()
    {
        throw new NotImplementedException();
    }
}
