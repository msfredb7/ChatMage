using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Phantom : StdCar
{
    public float unhitableDuration;

    public override void OnGameReady()
    {
        base.OnGameStarted();

        Game.Instance.Player.playerStats.sprite.color = Color.cyan;
        Game.Instance.Player.playerStats.unhitableDuration = unhitableDuration;
    }
}
