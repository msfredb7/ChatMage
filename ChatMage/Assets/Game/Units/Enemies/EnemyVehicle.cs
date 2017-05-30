using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyVehicle : Vehicle, IAttackable
{
    protected Vector2 targetPosition;
    protected bool goingTo = false;
    protected bool arrivedAtDestination = false;

    public void GotoPosition(Vector2 position)
    {
        targetPosition = position;
        goingTo = true;
    }
    public void GotoDirection(float direction)
    {
        goingTo = false;
        targetDirection = direction;
    }
    public void GotoDirection(Vector2 direction)
    {
        GotoDirection(VectorToAngle(direction));
    }

    protected override void FixedUpdate()
    {
        if (goingTo)
        {
            Vector2 v = targetPosition - rb.position;

            //Arrived ?
            if (v.magnitude > 0.01f) // Yes
            {
                if (arrivedAtDestination)
                {
                    arrivedAtDestination = false;
                    canAccelerate.Unlock("arrived");
                }
                targetDirection = VectorToAngle(targetPosition - rb.position);
            }
            else //No
            {
                if (!arrivedAtDestination) 
                {
                    if (canAccelerate)
                        rb.velocity = Vector3.zero;
                    arrivedAtDestination = true;
                    canAccelerate.Lock("arrived");
                }
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
        GotoPosition(rb.position);
    }

    public abstract int Attacked(ColliderInfo on, int amount, MonoBehaviour source);
}
