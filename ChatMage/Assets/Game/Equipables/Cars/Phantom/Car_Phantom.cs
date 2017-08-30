using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Phantom : StdCar
{
    public float unhitableDuration;

    public override void OnGameReady()
    {
        base.OnGameStarted();

        Game.instance.Player.playerStats.sprite.color = Color.cyan;
        Game.instance.Player.playerStats.unhitableDuration = unhitableDuration;
    }
}
