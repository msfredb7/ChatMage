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
            moveState = EnemyMoveState.FollowPlayer;
        }
        else if (dist > tooCloseRange && minFleeStay < 0)
        {
            //Attack or reload
            vehicle.WalkMode();
            vehicle.useTurnSpeed = true;
            moveState = EnemyMoveState.LookAtPlayer;
        }
        else
        {
            //flee
            if (moveState != EnemyMoveState.Flee)
                minFleeStay = 0.5f;

            vehicle.RunMode();
            vehicle.useTurnSpeed = false;
            minFleeStay -= vehicle.DeltaTime();
            moveState = EnemyMoveState.Flee;
        }
    }

    protected override void UpdateNoPlayer()
    {
        vehicle.useTurnSpeed = true;
        moveState = EnemyMoveState.Wander;
    }
}
