using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_RocketLockon : Item
{
    public RocketLauncher launcherPrefab;

    public override void Equip()
    {
        throw new NotImplementedException();
    }

    public override void OnGameReady()
    {
        Instantiate(launcherPrefab.gameObject, player.body);
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
