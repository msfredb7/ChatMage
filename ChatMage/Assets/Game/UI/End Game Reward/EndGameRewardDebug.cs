using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

public class EndGameRewardDebug : BaseBehavior {

    public EndGameReward.PinataExplosion explosion;

    [InspectorButton]
    public void Animate()
    {
        explosion.Animate(Vector2.up * 1.15f, EndGameReward.PinataExplosion.BallColor.Blue);
    }
}
