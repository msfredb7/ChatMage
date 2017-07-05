using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBehavior : EnemyBehavior<EnemyVehicle>
{
    public FollowBehavior(EnemyVehicle v) : base(v) { }

    public override BehaviorType Type { get { return BehaviorType.Follow; } }

    public override void Enter(Unit player)
    {
    }

    public override void Exit(Unit player)
    {
    }

    public override void Update(Unit player, float deltaTime)
    {
        if (player == null)
        {
            vehicle.Stop();
        }
        else
        {
            vehicle.GotoDirection(player.Position - vehicle.Position, deltaTime);
        }
    }
}
