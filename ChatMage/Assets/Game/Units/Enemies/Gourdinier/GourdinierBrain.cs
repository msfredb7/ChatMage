using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

public class GourdinierBrain : EnemyBrain<GourdinierVehicle>
{
    public float startAttackRange = 2;
    public float turnSpeed = 190;
    public bool movementPrediction = true;
    [InspectorTooltip("Il va prédire le mouvement du joueur dans x s."), InspectorShowIf("movementPrediction")]
    public float thinkAheadLength = 1;

    void Update()
    {
        if (player == null || !player.playerStats.isVisible || !player.gameObject.activeSelf)
            return;

        Vector2 meToP = (player.vehicle.Position - vehicle.Position);
        Vector2 meToPPredic = meToP + player.vehicle.Speed * thinkAheadLength;
        float dist = meToP.magnitude;

        if(dist <= startAttackRange ||(movementPrediction && meToPPredic.magnitude <= startAttackRange) || vehicle.isAttacking)
        {
            //Attack mode
            vehicle.Stop();
            if (vehicle.CanAttack())
                vehicle.Attack();

            //Turn towards player
            vehicle.Rotation = Mathf.MoveTowardsAngle(vehicle.Rotation, Vehicle.VectorToAngle(meToP), vehicle.DeltaTime() * turnSpeed);
        }
        else
        {
            //Go to player
            vehicle.GotoDirection(
                Mathf.MoveTowardsAngle(vehicle.Rotation, Vehicle.VectorToAngle(meToP), vehicle.DeltaTime() * turnSpeed));
        }

    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.25F);
        Gizmos.DrawSphere(transform.position, startAttackRange);
    }
}
