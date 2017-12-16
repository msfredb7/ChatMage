using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_Snuggie : Item
{

    public int armorBonus = 2;

    public override void Equip()
    {
        throw new NotImplementedException();
    }

    public override void OnGameReady()
    {
        player.playerStats.armor.Set(player.playerStats.armor + armorBonus);
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
