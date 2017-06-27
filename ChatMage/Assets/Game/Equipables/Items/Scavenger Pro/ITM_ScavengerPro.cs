using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_ScavengerPro : Item
{
    [FullInspector.InspectorRange(0, 100)]
    public float spawnChanceIncrease = 30;

    public override void Init(PlayerController player)
    {
        base.Init(player);
        Game.instance.healthPackManager.luckMultiplier += spawnChanceIncrease / 100;
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
