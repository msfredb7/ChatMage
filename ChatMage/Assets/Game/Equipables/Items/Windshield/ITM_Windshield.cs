using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_Windshield : Item, IAttackable
{
    public int Attacked(ColliderInfo on, int amount, Unit otherUnit, ColliderInfo source = null)
    {
        if (otherUnit is Projectiles && on == player.playerCarTriggers.frontCol)
            return 0;
        return amount;
    }

    public override void Init(PlayerController player)
    {
        base.Init(player);
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
