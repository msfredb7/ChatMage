using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehavior : EnemyBehavior<EnemyVehicle>
{
    public IdleBehavior(EnemyVehicle v) : base(v) { }

    public override void Enter(Unit player)
    {
    }

    public override void Exit(Unit player)
    {
    }

    public override void Update(Unit player, float deltaTime)
    {
        vehicle.Stop();
    }
}
