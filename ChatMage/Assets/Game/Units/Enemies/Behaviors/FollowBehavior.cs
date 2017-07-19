using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBehavior : EnemyBehavior<EnemyVehicle>
{
    public FollowBehavior(EnemyVehicle v) : base(v) { }

    public override void Enter(Unit target)
    {
    }

    public override void Exit(Unit target)
    {
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
}
