using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GourdinierBrain : EnemyBrain<GourdinierVehicle>
{
    public float attackRange = 2;
    public float turnSpeed = 190;

    void Update()
    {
        if (player == null || !player.playerStats.isVisible || !player.gameObject.activeSelf)
            return;

        Vector2 v = (player.vehicle.Position - vehicle.Position);
        float dist = v.magnitude;

        if (dist > 2 && !vehicle.isAttacking)
        {
            //Go to player
            vehicle.GotoDirection(
                Mathf.MoveTowardsAngle(vehicle.Rotation, Vehicle.VectorToAngle(v), vehicle.DeltaTime() * turnSpeed));
        }
        else
        {
            //Attack mode
            vehicle.Stop();
            if (vehicle.CanAttack())
                vehicle.Attack();
        }

    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.25F);
        Gizmos.DrawSphere(transform.position, attackRange);
    }
}
