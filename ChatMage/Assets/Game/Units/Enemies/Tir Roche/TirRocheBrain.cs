using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TirRocheBrain : EnemyBrain<TirRocheVehicle>
{
    public float attackingMaxRange = 6;
    public float tooCloseRange = 3;

    private float minFleeStay = -1;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 0, 1, 0.25F);
        Gizmos.DrawSphere(transform.position, attackingMaxRange);
        Gizmos.color = new Color(1, 0, 0, 0.25F);
        Gizmos.DrawSphere(transform.position, tooCloseRange);
    }

    protected override void UpdatePlayer()
    {
        float dist = meToPlayer.magnitude;
        if (dist > attackingMaxRange && minFleeStay < 0)
        {
            //Get closer or reload!
            vehicle.WalkMode();
            vehicle.useTurnSpeed = true;

            SetBehavior(BehaviorType.Follow);
        }
        else if (dist > tooCloseRange && minFleeStay < 0)
        {
            //Attack or reload
            vehicle.WalkMode();
            vehicle.useTurnSpeed = true;

            SetBehavior(BehaviorType.LookPlayer);
        }
        else
        {
            //flee
            if (CurrentBehaviorType != BehaviorType.Flee)
                minFleeStay = 0.5f;

            vehicle.RunMode();
            vehicle.useTurnSpeed = false;
            minFleeStay -= vehicle.DeltaTime();

            SetBehavior(BehaviorType.Flee);
        }
    }

    protected override void UpdateNoPlayer()
    {
        vehicle.useTurnSpeed = true;
        SetBehavior(BehaviorType.Wander);
    }
}
