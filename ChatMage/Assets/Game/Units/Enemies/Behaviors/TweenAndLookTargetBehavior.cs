using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TweenAndLookTargetBehavior : TweenBehavior
{
    public TweenAndLookTargetBehavior(EnemyVehicle vehicle, Tween tween)
        :base(vehicle, tween)
    {

    }

    public override void Enter(Unit target)
    {
        base.Enter(target);

        vehicle.Stop();
    }

    public override void Update(Unit target, float deltaTime)
    {
        base.Update(target, deltaTime);

        if (target != null)
            vehicle.TurnToDirection(target.Position - vehicle.Position, deltaTime);
    }
}
