using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookTargetBehavior : EnemyBehavior<EnemyVehicle>
{
    public LookTargetBehavior(EnemyVehicle v) : base(v) { }

    public override void Enter(Unit target)
    {
    }

    public override void Exit(Unit target)
    {
    }

    public override void Update(Unit target, float deltaTime)
    {
        vehicle.Stop();

        if (target != null)
            vehicle.TurnToDirection(target.Position - vehicle.Position, deltaTime);
    }
}
