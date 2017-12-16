using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_AOEBooster : Item
{
    public override void Equip()
    {
        throw new System.NotImplementedException();
    }

    public override void Init(PlayerController player)
    {
        base.Init(player);

        player.playerStats.boostedAOE = true;
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

    public override void Unequip()
    {
        throw new System.NotImplementedException();
    }
}
