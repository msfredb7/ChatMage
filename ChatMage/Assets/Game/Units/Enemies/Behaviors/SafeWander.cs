using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeWander : EnemyBehavior<EnemyVehicle>
{
    const float CHOOSE_INTERVAL = 3f;
    const float DISTANCE_MIN = 1.5f;
    const float DISTANCE_MAX = 3.5f;

    private float chooseTimer = 0;
    private float safeDistanceSqr;

    public SafeWander (EnemyVehicle v, float safeDistance = 3) : base(v)
    {
        this.safeDistanceSqr = safeDistance * safeDistance;
    }

    public override void Update(Unit target, float deltaTime)
    {
        if(target != null)
        {
            Vector2 meToTarget = target.Position - vehicle.Position;

            if(meToTarget.sqrMagnitude < safeDistanceSqr)
            {
                //Flee !
                vehicle.GotoDirection(-meToTarget, deltaTime);
                chooseTimer = 0;
            }
            else
            {
                if (chooseTimer < 0)
                    NewDestination();
            }
        }
        else
        {
            if (chooseTimer <= 0)
                NewDestination();
        }


        chooseTimer -= deltaTime;
    }

    void NewDestination()
    {
        Vector2 randomVector = CCC.Math.Vectors.RandomVector2(0, 360, DISTANCE_MIN, DISTANCE_MAX);

        vehicle.GotoPosition(vehicle.Position + randomVector);

        chooseTimer = Random.Range(CHOOSE_INTERVAL * 0.75f, CHOOSE_INTERVAL * 1.25f);
    }

    public override void Enter(Unit target)
    {
        chooseTimer = 0;
    }

    public override void Exit(Unit target)
    {

    }
}
