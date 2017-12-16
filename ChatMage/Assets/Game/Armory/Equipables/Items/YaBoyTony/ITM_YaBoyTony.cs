using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_YaBoyTony : Item
{
    public TonyVehicle tonyPrefab;

    public override void Equip(int duplicateIndex)
    {
        throw new NotImplementedException();
    }

    public override void OnGameReady()
    {
        Game.instance.SpawnUnit(tonyPrefab, Game.instance.gameCamera.Center + Vector2.right * 1.5f);
    }

    public override void OnGameStarted()
    {

    }

    public override void OnUpdate()
    {

    }

    public override void Unequip()
    {
        throw new NotImplementedException();
    }
}
