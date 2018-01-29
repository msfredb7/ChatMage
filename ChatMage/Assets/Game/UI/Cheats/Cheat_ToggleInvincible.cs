using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat_ToggleInvincible : CheatButton
{
    public override void Execute()
    {
        if (Game.Instance != null)
            Game.Instance.Player.playerStats.damagable = !Game.Instance.Player.playerStats.damagable;
    }
}
