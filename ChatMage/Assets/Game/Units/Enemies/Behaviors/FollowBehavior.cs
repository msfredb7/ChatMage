using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBehavior : EnemyBehavior<EnemyVehicle>
{
    public FollowBehavior(EnemyVehicle v) : base(v) { }

    public override BehaviorType Type { get { return BehaviorType.Follow; } }

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
            vehicle.GotoDirection(player.vehicle.Position - vehicle.Position, deltaTime);
        }
    }
}
