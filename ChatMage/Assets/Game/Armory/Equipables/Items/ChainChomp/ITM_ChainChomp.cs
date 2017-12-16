using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_ChainChomp : Item
{
    public ChainChomp prefab;

    [FullSerializer.fsIgnore]
    private ChainChomp chainChomp;

    public override void Init(PlayerController player)
    {
        base.Init(player);
        chainChomp = Game.instance.SpawnUnit(prefab, player.vehicle.Position);
        chainChomp.Init(player.playerLocations.boule, player);
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

    public override void Equip()
    {
        throw new NotImplementedException();
    }

    public override void Unequip()
    {
        throw new NotImplementedException();
    }
}
