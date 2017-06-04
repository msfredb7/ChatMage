using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderBehavior : EnemyBehavior<EnemyVehicle>
{
    const float CHOOSE_INTERVAL = 4f;
    const float DISTANCE_MIN = 0.75f;
    const float DISTANCE_MAX = 3.5f;

    float chooseTimer = 0;
    float lastDirectionPick = 0;

    public WanderBehavior(EnemyVehicle v) : base(v) { }

    public override BehaviorType Type { get { return BehaviorType.Wander; } }

    public override void Enter(PlayerController player)
    {
    }

    public override void Exit(PlayerController player)
    {
    }

    public override void Update(PlayerController player, float deltaTime)
    {
        chooseTimer -= deltaTime;

        if (chooseTimer < 0)
        {
            //Pick new destination
            if (lastDirectionPick > 360)
                lastDirectionPick -= 360;

            Vector2 randomVector = Vehicle.AngleToVector(Random.Range(0, 360));

            vehicle.GotoPosition(vehicle.Position + randomVector * Random.Range(DISTANCE_MIN, DISTANCE_MAX));

            chooseTimer = CHOOSE_INTERVAL;
        }
    }
}
