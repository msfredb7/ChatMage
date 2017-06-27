using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_CooldownReduction : Item
{
    [Range(0, 1)]
    public float cooldownReduction = 0.5f;

    public override void OnGameReady()
    {
        Game.instance.Player.playerStats.cooldownMultiplier.Set(cooldownReduction);
    }

    public override void OnGameStarted()
    {
    }

    public override void OnUpdate()
    {
    }

    void LaunchBomb()
    {
    }
}
