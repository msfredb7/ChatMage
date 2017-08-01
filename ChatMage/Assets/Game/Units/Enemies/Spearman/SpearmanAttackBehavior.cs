using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpearmanAttackBehavior : BaseTweenBehavior<SpearmanVehicle>
{
    private bool lookAtTarget = true;

    public SpearmanAttackBehavior(SpearmanVehicle spearman) :
        base(spearman)
    {
        tween = spearman.animator.AttackAnimation(OnAttackMoment).OnComplete(OnComplete);
    }

    private void OnAttackMoment()
    {
        lookAtTarget = false;
    }

    public override void Enter(Unit target)
    {
        base.Enter(target);

        //vehicle.AttackStarted();
        vehicle.Stop();
    }

    void OnComplete()
    {
        //vehicle.AttackCompleted();
    }

    public override void Update(Unit target, float deltaTime)
    {
        if (lookAtTarget && target != null)
            vehicle.TurnToDirection(target.Position - vehicle.Position, deltaTime);
    }
}
