using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_MonsterTruck : Item
{
    public float scaleAmp = 1;
    public int hpAmp = 2;

    public override void Init(PlayerController player)
    {
        base.Init(player);
    }

    public override void OnGameReady()
    {
        Transform body = Game.instance.Player.body;
        body.localScale = new Vector3(body.localScale.x + scaleAmp, body.localScale.y + scaleAmp, body.localScale.z + scaleAmp);
        Game.instance.Player.playerStats.health.MAX = Game.instance.Player.playerStats.health * hpAmp;
        Game.instance.Player.playerStats.health.Set(Game.instance.Player.playerStats.health.MAX);
    }

    public override void OnGameStarted()
    {

    }

    public override void OnUpdate()
    {

    }
}
