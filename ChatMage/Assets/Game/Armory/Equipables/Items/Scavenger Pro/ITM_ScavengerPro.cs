using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

public class ITM_ScavengerPro : Item
{
    [InspectorHeader("Health packs"), InspectorRange(1, 2)]
    public float appearanceMultiplier = 1.35f;

    public override void Equip(int duplicateIndex)
    {
        throw new System.NotImplementedException();
    }

    public override void Init(PlayerController player)
    {
        base.Init(player);
        Game.instance.healthPackManager.luckMultiplier *= appearanceMultiplier;
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

    public override void Unequip()
    {
        throw new System.NotImplementedException();
    }
}
