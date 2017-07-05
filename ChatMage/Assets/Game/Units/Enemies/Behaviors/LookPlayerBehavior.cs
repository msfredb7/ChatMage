using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookPlayerBehavior : EnemyBehavior<EnemyVehicle>
{
    public LookPlayerBehavior(EnemyVehicle v) : base(v) { }

    public override BehaviorType Type { get { return BehaviorType.LookPlayer; } }

    public override void Enter(Unit player)
    {
    }

    public override void Exit(Unit player)
    {
    }

    public override void Update(Unit player, float deltaTime)
    {
        vehicle.Stop();

        if (player != null)
            vehicle.TurnToDirection(player.Position - vehicle.Position, deltaTime);
    }
}
