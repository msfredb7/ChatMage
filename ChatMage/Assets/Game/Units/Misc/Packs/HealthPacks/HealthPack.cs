using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HealthPack : Pack
{
    [System.NonSerialized]
    public int healValue = 1;

    protected override void OnPickUp()
    {
        Game.instance.Player.playerStats.GiveHealth(healValue);
    }
}
