using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeBehavior : EnemyBehavior<EnemyVehicle>
{
    public FleeBehavior(EnemyVehicle v) : base(v) { }

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
            vehicle.GotoDirection(vehicle.Position - player.Position, deltaTime);
        }
    }
}
