using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehavior : EnemyBehavior
{
    public IdleBehavior(EnemyVehicle v) : base(v) { }

    public override BehaviorType Type { get { return BehaviorType.Idle; } }

    public override void Enter(PlayerController player)
    {
    }

    public override void Exit(PlayerController player)
    {
    }

    public override void Update(PlayerController player, float deltaTime)
    {
        vehicle.Stop();
    }
}
