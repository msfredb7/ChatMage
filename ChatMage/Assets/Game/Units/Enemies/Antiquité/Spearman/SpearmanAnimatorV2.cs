using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearmanAnimatorV2 : EnemyAnimator
{
    public SpearmanVehicle vehicle;

    private int attackHash = Animator.StringToHash("attack");
    private int movingHash = Animator.StringToHash("moving");
    private Action attackCallback;
    private Action attackMoment;
    private bool moving = false;

    protected override EnemyVehicle Vehicle
    {
        get
        {
            return vehicle;
        }
    }

    void Update()
    {
        if(vehicle.Speed.sqrMagnitude < 0.1f)
        {
            if (moving)
            {
                moving = false;
                controller.SetBool(movingHash, false);
            }
        }
        else
        {
            if (!moving)
            {
                moving = true;
                controller.SetBool(movingHash, true);
            }
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
}
