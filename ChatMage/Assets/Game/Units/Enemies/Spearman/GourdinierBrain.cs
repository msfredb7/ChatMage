using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using System;

public class GourdinierBrain : EnemyBrain<GourdinierVehicle>
{
    [InspectorHeader("Gourdinier Brain")]
    public float startAttackRange = 2;
    public bool movementPrediction = true;
    [InspectorTooltip("Il va prédire le mouvement du joueur dans x s."), InspectorShowIf("movementPrediction")]
    public float thinkAheadLength = 1;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.25F);
        Gizmos.DrawSphere(transform.position, startAttackRange);
    }

    protected override void UpdateWithTarget()
    {
        Vector2 meToPPredic = meToTarget + target.Speed * thinkAheadLength;
        float dist = meToTarget.magnitude;

        if (dist <= startAttackRange || (movementPrediction && meToPPredic.magnitude <= startAttackRange) || vehicle.isAttacking)
        {
            //Attack mode
            if (CanGoTo<LookTargetBehavior>())
                SetBehavior(new LookTargetBehavior(vehicle));

            if (vehicle.CanAttack())
                vehicle.Attack();
        }
        else
        {
            //Go to player
            if (CanGoTo<FollowBehavior>())
                SetBehavior(new FollowBehavior(vehicle));
        }
    }

    protected override void UpdateWithoutTarget()
    {
        if (CanGoTo<WanderBehavior>())
            SetBehavior(new WanderBehavior(vehicle));
    }
}
