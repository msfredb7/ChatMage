using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeBehavior : EnemyBehavior<EnemyVehicle>
{
    public FleeBehavior(EnemyVehicle v) : base(v) { }

    public override BehaviorType Type { get { return BehaviorType.Flee; } }

    public override void Enter(PlayerController player)
    {
    }

    public override void Exit(PlayerController player)
    {
    }

    public override void Update(PlayerController player, float deltaTime)
    {
        if (player == null)
        {
            vehicle.Stop();
        }
        else
        {
            vehicle.GotoDirection(vehicle.Position - player.vehicle.Position, deltaTime);
        }
    }
}
