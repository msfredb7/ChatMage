using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearmanAnimatorV2 : EnemyAnimator
{
    public SpearmanVehicle vehicle;

    private int attackHash = Animator.StringToHash("attack");
    private int deadHash = Animator.StringToHash("dead");
    private Action attackCallback;
    private Action attackMoment;
    private Action deathCallback;

    public void DeathAnimation(Action onComplete)
    {
        deathCallback = onComplete;
        controller.SetTrigger(deadHash);
    }

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
    }

    private void _AttackMoment()
    {
        if (attackMoment != null)
            attackMoment();
    }
    private void _AttackComplete()
    {
        if (attackCallback != null)
            attackCallback();
    }
    private void _DeathComplete()
    {
        if (deathCallback != null)
            deathCallback();
    }
}
