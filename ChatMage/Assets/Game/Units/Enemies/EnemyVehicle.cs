using System;
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
    protected Action onReach = null;


    #region Public Move Methods
    public void Stop()
    {
        EngineOff();
        goingToTargetPosition = false;
    }

    public void GoAndStayAtPosition(Vector2 position, Action onReach = null, bool restrainToScreenIfApplies = true)
    {
        tryToStayAtTargetPosition = true;
        QuickGotoPos(position, onReach, restrainToScreenIfApplies);
    }

    public void GotoPosition(Vector2 position, Action onReach = null, bool restrainToScreenIfApplies = true)
    {
        tryToStayAtTargetPosition = false;
        QuickGotoPos(position, onReach, restrainToScreenIfApplies);
    }

    private void QuickGotoPos(Vector2 position, Action onReach = null, bool restrainToScreenIfApplies = true)
    {
        if (restrainToScreenIfApplies)
        {
            position = RestrainToBounds(position);
        }

        targetPosition = position;
        goingToTargetPosition = true;
        this.onReach = onReach;
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
            if (v.magnitude > 0.3f)
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

                if (onReach != null)
                {
                    onReach();
                    onReach = null;
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

    public override void SaveRigidbody()
    {
        targetPosition = Position - targetPosition;
        base.SaveRigidbody();
    }

    public override void LoadRigidbody()
    {
        targetPosition = Position + targetPosition;
        base.LoadRigidbody();
    }

    public abstract int Attacked(ColliderInfo on, int amount, Unit unit, ColliderInfo source = null);
}
