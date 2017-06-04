using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookPlayerBehavior : EnemyBehavior
{
    public LookPlayerBehavior(EnemyVehicle v) : base(v) { }

    public override BehaviorType Type { get { return BehaviorType.LookPlayer; } }

    public override void Enter(PlayerController player)
    {
    }

    public override void Exit(PlayerController player)
    {
    }

    public override void Update(PlayerController player, float deltaTime)
    {
        vehicle.Stop();

        if (player != null)
            vehicle.TurnToDirection(player.vehicle.Position - vehicle.Position, deltaTime);
    }
}
