using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShielderFollowBehavior : EnemyBehavior<ShielderVehicle>
{
    private Action onCanAttack;

    public ShielderFollowBehavior(ShielderVehicle vehicle, Action onCanAttack) : base(vehicle)
    {
        this.onCanAttack = onCanAttack;
    }

    public override void Enter(Unit target)
    {
        vehicle.onShieldPhysicalHit += OnShieldPhysicalHit;
    }

    void OnShieldPhysicalHit(Unit unitHit)
    {
        if (vehicle.targets.IsValidTarget(unitHit.allegiance))
        {
            onCanAttack();
        }
    }

    public override void Update(Unit target, float deltaTime)
    {
        if (target == null)
        {
            vehicle.Stop();
        }
        else
        {
            vehicle.GotoPosition(target.Position);
        }
    }

    public override void Exit(Unit target)
    {
        vehicle.onShieldPhysicalHit -= OnShieldPhysicalHit;
    }
}
