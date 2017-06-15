using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_ScavengerPro : Item
{
    public override void Init(PlayerController player)
    {
        base.Init(player);
        Game.instance.healthPackManager.spawnChance = Game.instance.healthPackManager.lerpCeiling;
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
