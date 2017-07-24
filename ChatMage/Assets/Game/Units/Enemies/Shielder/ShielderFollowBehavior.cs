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
            Vector2 meToTarget = target.Position - vehicle.Position;
            float meToTargetAngle = meToTarget.ToAngle();
            float angleDelta = (meToTargetAngle - vehicle.Rotation).Abs();

            if (angleDelta > 70)
            {
                vehicle.Stop();
                vehicle.TurnToDirection(meToTargetAngle, deltaTime);
            }
            else
            {
                vehicle.GotoDirection(meToTargetAngle, deltaTime);
            }
        }
    }

    public override void Exit(Unit target)
    {
        vehicle.onShieldPhysicalHit -= OnShieldPhysicalHit;
    }
}
