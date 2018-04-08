using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearmanAnimatorV2 : EnemyAnimator
{
    public SpearmanVehicle vehicle;

    private int attackHash = Animator.StringToHash("attack");
    private Action attackCallback;
    private Action attackMoment;

    protected override EnemyVehicle Vehicle
    {
        get
        {
            return vehicle;
        }
    }

    public void AttackAnimation(Action attackMoment, Action onComplete)
    {
        attackCallback = onComplete;
        this.attackMoment = attackMoment;
        controller.SetTrigger(attackHash);
        vehicle.spearAttackConsumed = false;
    }

    public void _AttackMoment()
    {
        if (attackMoment != null)
            attackMoment();
    }
    public void _AttackComplete()
    {
        if (attackCallback != null)
            attackCallback();
    }
}
