using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVehicle : Vehicle
{
    protected bool arrived = false;
    protected Vector2 targetPosition;

    public void Goto(Vector2 position)
    {
        targetPosition = position;
    }

    protected override void FixedUpdate()
    {
        Vector2 v = targetPosition - rb.position;
        if (v.magnitude > 0.01f)
        {
            if (arrived)
            {
                arrived = false;
                canAccelerate.Unlock("arrived");
            }
            targetDirection = VectorToAngle(targetPosition - rb.position);
        }
        else
        {
            if (!arrived)
            {
                if (canAccelerate)
                    rb.velocity = Vector3.zero;
                arrived = true;
                canAccelerate.Lock("arrived");
            }
        }

        base.FixedUpdate();
    }

    public Vector2 GetPositionAwayFromPlayer(float length)
    {
        Vector2 v = rb.position - Game.instance.Player.vehicle.Position;
        if (v.magnitude > 0.01f)
            return v.normalized * length;
        else
            return Vector2.up * length;
    }

    public void Idle()
    {
        Goto(rb.position);
    }
}
