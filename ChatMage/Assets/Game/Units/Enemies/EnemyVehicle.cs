using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyVehicle : Vehicle, IAttackable
{
    [Header("Enemy Vehicle")]
    public bool useTurnSpeed;
    public float turnSpeed = 150;

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
        goingToTargetPosition = true;
    }

    public void GotoPosition(Vector2 position)
    {
        targetPosition = position;
        tryToStayAtTargetPosition = false;
        goingToTargetPosition = true;
    }

    public void GotoDirection(float direction, float deltaTime)
    {
        EngineOn();
        goingToTargetPosition = false;
        TurnToDirection(direction, deltaTime);
    }

    public void GotoDirection(Vector2 direction, float deltaTime)
    {
        GotoDirection(VectorToAngle(direction), deltaTime);
    }
    #endregion

    #region Internal Move Methods
    public void TurnToDirection(float direction, float deltaTime)
    {
        if (useTurnSpeed)
        {
            if (rotationSetsTargetDirection)
                Rotation = Mathf.MoveTowardsAngle(Rotation, direction, turnSpeed * deltaTime);
            else
                targetDirection = Mathf.MoveTowardsAngle(targetDirection, direction, turnSpeed * deltaTime);
        }
        else
        {
            if (rotationSetsTargetDirection)
                Rotation = direction;
            else
                targetDirection = direction;
        }
    }
    public void TurnToDirection(Vector2 direction, float deltaTime)
    {
        TurnToDirection(VectorToAngle(direction), deltaTime);
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

                TurnToDirection(targetPosition - rb.position, FixedDeltaTime());
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
