using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using System;

public class SpearmanBrain : EnemyBrain<SpearmanVehicle>
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
        //Si on est entrain d'attaquer, on ne fait pas de changement a notre behavior
        if (!vehicle.CanAttack)
            return;

        Vector2 meToPPredic = meToTarget;
        if (target is MovingUnit)
            meToPPredic += (target as MovingUnit).Speed * thinkAheadLength;

        float dist = meToTarget.magnitude;

        if (dist <= startAttackRange ||  //En range d'attaque
            (movementPrediction && meToPPredic.sqrMagnitude <= startAttackRange * startAttackRange))
        {

            //Attack mode
            if (vehicle.CanAttack)
            {
                SetBehavior(new SpearmanAttackBehavior(vehicle));
            }
            else if (CanGoTo<LookTargetBehavior>())
            {
                SetBehavior(new LookTargetBehavior(vehicle));
            }
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
