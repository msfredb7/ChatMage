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

    protected override void UpdatePlayer()
    {
        Vector2 meToPPredic = meToPlayer + player.vehicle.Speed * thinkAheadLength;
        float dist = meToPlayer.magnitude;

        if (dist <= startAttackRange || (movementPrediction && meToPPredic.magnitude <= startAttackRange) || vehicle.isAttacking)
        {
            //Attack mode
            moveState = EnemyMoveState.LookAtPlayer;

            if (vehicle.CanAttack())
                vehicle.Attack();
        }
        else
        {
            //Go to player
            moveState = EnemyMoveState.FollowPlayer;
        }
    }

    protected override void UpdateNoPlayer()
    {
        moveState = EnemyMoveState.Wander;
    }
}
