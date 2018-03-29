using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyVehicle : Vehicle, IAttackable
{
    [Header("Enemy Vehicle")]
    public float unitWidth;
    public bool useTurnSpeed;
    public float turnSpeed = 150;
    public bool smartMove = false;
    public bool clampToAIArea = true;
    public int smashJuice = 1;
    [Forward]
    public Targets targets;

    [Header("VFX")]
    public Transform bodyCenter;

    protected Vector2 targetPosition;
    protected bool goingToTargetPosition = false;
    protected Action onReach = null;
    private float gotoMaxDurationTimer;

    public bool IsEnginOn { get { return currentMoveSpeed > 0; } }

    protected override void Awake()
    {
        base.Awake();
        EngineOff();
    }


    #region Public Move Methods
    public void Stop()
    {
        EngineOff();
        goingToTargetPosition = false;
        onReach = null;
    }

    public void GotoPosition(Vector2 position, Action onReach = null, float maximumAllowedDuration = -1)
    {
        QuickGotoPos(position, onReach, maximumAllowedDuration);
    }

    private void QuickGotoPos(Vector2 position, Action onReach, float maximumAllowedDuration)
    {
        //Clamp to AI Area
        if (clampToAIArea && Game.Instance != null)
            position = Game.Instance.aiArea.ClampToArea(position, unitWidth / 2);

        if (smartMove)
            position = NAV_SmartMover.SmartifyMove(Position, position, unitWidth);

        gotoMaxDurationTimer = maximumAllowedDuration;

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
            // Timer pour s'assurer qu'on seek pas une position pour l'infinie
            bool wasInTimer = gotoMaxDurationTimer > 0;
            gotoMaxDurationTimer -= Time.fixedDeltaTime;
            bool isInTimer = gotoMaxDurationTimer > 0;
            bool timerHasJustEnded = wasInTimer && !isInTimer;

            Vector2 v = targetPosition - rb.position;

            //Distance to target ?
            if (v.sqrMagnitude > 0.09f && !timerHasJustEnded)
            {
                float vAngle = v.ToAngle();
                float angleDelta = (Rotation - vAngle).Abs();

                // Going to
                if (angleDelta < 70)
                    EngineOn();
                else
                    EngineOff();

                TurnToDirection(vAngle, FixedDeltaTime());
            }
            else
            {
                EngineOff();

                Action wasOnReach = onReach;

                //Arrived !
                Stop();

                if (wasOnReach != null)
                {
                    wasOnReach();
                }
            }
        }

        base.FixedUpdate();
    }

    #endregion

    protected virtual void OnDrawGizmosSelected()
    {
        if (goingToTargetPosition)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(Position, targetPosition);
        }
    }

    public Vector2 GetPositionAwayFromPlayer(float length)
    {
        Vector2 v = rb.position - Game.Instance.Player.vehicle.Position;
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

    public abstract int Attacked(ColliderInfo on, int amount, Unit otherUnit, ColliderInfo source = null);

    public float GetSmashJuiceReward()
    {
        return smashJuice;
    }
}
