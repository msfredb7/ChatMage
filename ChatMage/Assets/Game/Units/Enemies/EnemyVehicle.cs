using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyVehicle : Vehicle, IAttackable
{
    protected bool tryToStayAtTargetPosition = false;
    protected Vector2 targetPosition;
    protected bool goingToTargetPosition = false;
    protected bool arrivedAtDestination = false;


    #region Public Move Methods
    public void Stop()
    {
        EngineOff();
        goingToTargetPosition = false;
    }

    public void GoAndStayAtPosition(Vector2 position)
    {
        targetPosition = position;
        tryToStayAtTargetPosition = true;
    }

    public void GotoPosition(Vector2 position)
    {
        targetPosition = position;
        tryToStayAtTargetPosition = false;
    }

    public void GotoDirection(float direction)
    {
        EngineOn();
        goingToTargetPosition = false;
        SetDirection(direction);
    }

    public void GotoDirection(Vector2 direction)
    {
        GotoDirection(VectorToAngle(direction));
    }
    #endregion

    #region Internal Move Methods
    protected void SetDirection(float direction)
    {
        if (rotationSetsTargetDirection)
            Rotation = direction;
        else
            targetDirection = direction;
    }
    protected void SetDirection(Vector2 direction)
    {
        SetDirection(VectorToAngle(direction));
    }

    protected override void FixedUpdate()
    {
        if (goingToTargetPosition)
        {
            Vector2 v = targetPosition - rb.position;

            //Distance to target ?
            if (v.magnitude > 0.01f)
            {
                // Going to
                EngineOn();

                SetDirection(targetPosition - rb.position);
            }
            else
            {
                EngineOff();

                //Arrived !
                if (!tryToStayAtTargetPosition)
                {
                    //Dont have to stay...
                    Stop();
                }
            }
        }

        base.FixedUpdate();
    }

    #endregion
    public Vector2 GetPositionAwayFromPlayer(float length)
    {
        Vector2 v = rb.position - Game.instance.Player.vehicle.Position;
        if (v.magnitude > 0.01f)
            return v.normalized * length;
        else
            return Vector2.up * length;
    }
    public abstract int Attacked(ColliderInfo on, int amount, MonoBehaviour source);
}
