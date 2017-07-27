using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SwordsmanAttackBehavior : BaseTweenBehavior<SwordsmanVehicle>
{
    private bool lookAtTarget = true;

    public SwordsmanAttackBehavior(SwordsmanVehicle spearman) :
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

        vehicle.AttackStarted();
        vehicle.Stop();
    }

    protected override void OnCancel()
    {
        base.OnCancel();
        vehicle.AttackEnded();
    }

    void OnComplete()
    {
        vehicle.AttackEnded();
    }

    public override void Update(Unit target, float deltaTime)
    {
        if (lookAtTarget && target != null)
            vehicle.TurnToDirection(target.Position - vehicle.Position, deltaTime);
    }
}
