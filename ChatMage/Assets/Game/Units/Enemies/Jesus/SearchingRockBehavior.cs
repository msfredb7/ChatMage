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

    public override void Enter(PlayerController player)
    {
        vehicle.GoToLocation(rockTarget.Position);
    }

    public override void Exit(PlayerController player)
    {
    }

    public override void Update(PlayerController player, float deltaTime)
    {
    }
}
