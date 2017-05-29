using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_DblDmg : Item
{

    public override void Init(PlayerController player)
    {
        base.Init(player);

        player.playerStats.damage.Set(player.playerStats.damage * 2);

        // Ajouter les collider
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
