using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_ChainChomp : Item
{
    public ChainChomp prefab;

    [FullSerializer.fsIgnore]
    private ChainChomp chainChompInstance;

    public override void OnGameReady()
    {
    }

    public override void OnGameStarted()
    {
    }

    public override void OnUpdate()
    {
    }

    public override void Equip(int duplicateIndex)
    {
        chainChompInstance = Game.instance.SpawnUnit(prefab, player.vehicle.Position);
        chainChompInstance.Init(player.playerLocations.boule, player);
        chainChompInstance.Spawn();
    }

    public override void Unequip()
    {
        chainChompInstance.DetachAndDisapear();
    }
}
