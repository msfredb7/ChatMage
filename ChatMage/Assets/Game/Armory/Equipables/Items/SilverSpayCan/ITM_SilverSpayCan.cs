﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_SilverSpayCan : Item
{
    public override void Equip()
    {
        throw new System.NotImplementedException();
    }

    public override void OnGameReady()
    {
    }

    public override void OnGameStarted()
    {
        // Pour etre sur que le healthpackmanager a fait son init
        Game.instance.healthPackManager.spawnArmor = true;
    }

    public override void OnUpdate()
    {
    }

    public override void Unequip()
    {
        throw new System.NotImplementedException();
    }
}
