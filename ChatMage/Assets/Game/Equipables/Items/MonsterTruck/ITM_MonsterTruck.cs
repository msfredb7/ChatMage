using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_MonsterTruck : Item
{
    public float scaleMultiplier = 2;
    public float hpMultiplier = 2;

    public override void Init(PlayerController player)
    {
        base.Init(player);
    }

    public override void OnGameReady()
    {
        //NOTE: �a serait peut-etre mieux de scale individuellement les partie du char qu'on veut grossir
        //      Sinon, on risque de grossir des chose non voulue (ex: des prefab de d'autres items)

        Transform body = player.body;
        body.localScale = Vector3.Scale(body.localScale, Vector3.one * scaleMultiplier);

        player.playerStats.health.MAX = Mathf.RoundToInt((float)player.playerStats.health * hpMultiplier);
        player.playerStats.health.Set(player.playerStats.health.MAX);
    }

    public override void OnGameStarted()
    {

    }

    public override void OnUpdate()
    {

    }
}
