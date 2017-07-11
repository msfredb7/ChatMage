using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

public class ShielderBrain : EnemyBrain<ShielderVehicle>
{
    protected override void UpdateWithoutTarget()
    {
        if (CanGoTo<WanderBehavior>())
            SetBehavior(new WanderBehavior(vehicle));
        vehicle.PassiveMode();
    }

    protected override void UpdateWithTarget()
    {
        if(!IsBehavior<ShielderAttackBehavior>() && !IsBehavior<ShielderFollowBehavior>())
        {
            SetBehavior(new ShielderFollowBehavior(vehicle, OnCanAttack));
        }
        vehicle.BattleMode();
    }

    private void OnCanAttack()
    {
        SetBehavior(new ShielderAttackBehavior(vehicle, OnAttackComplete));
    }

    private void OnAttackComplete()
    {
        SetBehavior(new ShielderFollowBehavior(vehicle, OnCanAttack));
    }
}
