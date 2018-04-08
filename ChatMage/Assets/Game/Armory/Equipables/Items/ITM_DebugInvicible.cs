using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_DebugInvicible : Item
{
    public override void Equip(int duplicateIndex)
    {
        Game.Instance.Player.playerStats.damagable = false;
    }

    public override void OnGameReady()
    {
        player.playerStats.damagable = false;
    }

    public override void OnGameStarted()
    {
    }

    public override void OnUpdate()
    {
    }

    public override void Unequip()
    {
        Game.Instance.Player.playerStats.damagable = true;
    }
}
