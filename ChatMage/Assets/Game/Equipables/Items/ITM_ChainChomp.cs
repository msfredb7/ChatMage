using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_ChainChomp : Item
{
    public ChainChomp prefab;

    [FullSerializer.fsIgnore]
    private ChainChomp chainChomp;

    public override void OnGameReady()
    {
        chainChomp = Game.instance.SpawnUnit(prefab, Game.instance.Player.vehicle.Position);
        chainChomp.Init(Game.instance.Player.playerLocations.boule);
    }

    public override void OnGameStarted()
    {
    }

    public override void OnUpdate()
    {
    }
}
