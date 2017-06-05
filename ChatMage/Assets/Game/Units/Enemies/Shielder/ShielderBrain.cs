using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

public class ShielderBrain : EnemyBrain<ShielderVehicle>
{
    [InspectorHeader("Attack angle")]
    public float attackAngle = 25;
    public float attackDelay = 0;

    protected override void Start()
    {
        base.Start();
        vehicle.onShieldPhysicalHit += OnShieldPhysicalHit;
    }

    void OnShieldPhysicalHit(Unit unitHit)
    {
        if (vehicle.CanBumpShield)
            vehicle.BumpShield(unitHit, delegate()
            {
                if (unitHit != null && Mathf.DeltaAngle(Vehicle.VectorToAngle(unitHit.Position - vehicle.Position), vehicle.Rotation) < 25)
                    vehicle.Attack();
            }, attackDelay);
    }

    protected override void UpdateNoPlayer()
    {
        SetBehavior(BehaviorType.Wander);
        vehicle.PassiveMode();
    }

    protected override void UpdatePlayer()
    {
        float angleToPlayer = Mathf.DeltaAngle(Vehicle.VectorToAngle(player.vehicle.Position - vehicle.Position), vehicle.Rotation);
        if (angleToPlayer > 75)
            SetBehavior(BehaviorType.LookPlayer);
        else
            SetBehavior(BehaviorType.Follow);
        vehicle.BattleMode();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 1, 0.25F);
        Vector3 v1 = Vehicle.AngleToVector(attackAngle / 2 + GetComponent<Rigidbody2D>().rotation);
        Vector3 v2 = Vehicle.AngleToVector(-attackAngle / 2 + GetComponent<Rigidbody2D>().rotation);
        Gizmos.DrawLine(transform.position, transform.position + v1 * 3);
        Gizmos.DrawLine(transform.position, transform.position + v2 * 3);
    }
}
