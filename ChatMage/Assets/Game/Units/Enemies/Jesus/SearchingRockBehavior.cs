using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchingRockBehavior : EnemyBehavior<JesusVehicle>
{
    public SearchingRockBehavior(JesusVehicle v, JesusRock rockTarget) : base(v) { this.rockTarget = rockTarget; }

    private JesusRock rockTarget;

    public override BehaviorType Type
    {
        get
        {
            return BehaviorType.Wander;
        }
    }

    public override void Enter(Unit target)
    {
        vehicle.GoToLocation(rockTarget.Position);
    }

    public override void Exit(Unit target)
    {
    }

    public override void Update(Unit target, float deltaTime)
    {
    }
}
