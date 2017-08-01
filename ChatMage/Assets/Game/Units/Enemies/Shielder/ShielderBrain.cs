using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

public class ShielderBrain : EnemyBrain<ShielderVehicle>
{
    private bool isAttacking = false;
    protected override void UpdateWithoutTarget()
    {
        if (!isAttacking && CanGoTo<WanderBehavior>())
            SetBehavior(new WanderBehavior(vehicle));
        vehicle.PassiveMode();
    }

    protected override void UpdateWithTarget()
    {
        if(!isAttacking && !IsBehavior<ShielderFollowBehavior>() && !IsForcedIntoState)
        {
            SetBehavior(new ShielderFollowBehavior(vehicle, OnCanAttack));
        }
        vehicle.BattleMode();
    }

    private void OnCanAttack()
    {
        vehicle.Stop();

        isAttacking = true;
        TweenBehavior attackBehavior = new TweenBehavior(vehicle, vehicle.animator.AttackAnimation());
        attackBehavior.onComplete = OnAttackComplete;
        attackBehavior.onCancel = OnAttackEnd;

        SetBehavior(attackBehavior);
    }

    private void OnAttackComplete()
    {
        OnAttackEnd();
        SetBehavior(new ShielderFollowBehavior(vehicle, OnCanAttack));
    }

    private void OnAttackEnd()
    {
        isAttacking = false;
    }
}
