using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class ShielderAttackBehavior : BaseTweenBehavior<ShielderVehicle>
{
    public ShielderAttackBehavior (ShielderVehicle vehicle, TweenCallback onComplete) : base(vehicle)
    {
        vehicle.Stop();
        SetTween(vehicle.animator.AttackAnimation().OnComplete(onComplete));
    }

    public override void Update(Unit target, float deltaTime)
    {

    }
}
