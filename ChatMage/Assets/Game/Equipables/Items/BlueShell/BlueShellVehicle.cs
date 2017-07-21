using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueShellVehicle : Vehicle
{
    public SimpleColliderListener colliderListener;
    public event SimpleEvent onDeactivate;

    [Header("Blue Shell")]
    public BlueShellAnimator animator;

    [Header("Behavior")]
    public float wanderDuration;
    public float screenBorderWidth = 1.5f;

    [Header("Movement")]
    public float maxTurnSpeed = 500;
    public float maxTurnAcceleration = 2000;
    public float minPickRate = 0.2f;
    public float maxPickRate = 0.3f;

    private Unit target = null;

    private bool wandering = true;
    private float wanderingRemains;

    private float chooseNewTurnAcc;
    private float turnAcc;
    private float turnSpeed;

    protected override void Awake()
    {
        base.Awake();
        colliderListener.onTriggerEnter += ColliderListener_onTriggerEnter;

        onTimeScaleChange += BlueShellScript_onTimeScaleChange;
    }

    private void BlueShellScript_onTimeScaleChange(Unit unit)
    {
        if (GetComponent<TrailRenderer>() != null)
            GetComponent<TrailRenderer>().time = 0.2f / TimeScale;
    }

    public void ResetValues(Vector2 position)
    {
        if (Game.instance.Player != null)
            Rotation = Game.instance.Player.vehicle.Rotation;

        gameObject.SetActive(true);
        enabled = true;
        tr.position = position;
        target = null;
        wandering = true;
        wanderingRemains = wanderDuration;
        turnAcc = 0;
        turnSpeed = 0;
        chooseNewTurnAcc = 0;

        animator.ResetValues();
    }

    protected override  void Update()
    {
        if (wandering && wanderingRemains < 0)
            wandering = false;

        wanderingRemains -= DeltaTime();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (wandering)
        {
            Wander(FixedDeltaTime());
        }
        else
        {
            //If target is null, find new one
            if (target == null)
                target = FindClosestTargetTo(transform.position);

            if (target != null)
            {
                Rotation = Mathf.MoveTowardsAngle(Rotation,
                    VectorToAngle(target.Position - Position),
                    maxTurnSpeed * FixedDeltaTime());
            }
            else
                Wander(FixedDeltaTime());
        }
    }

    private void Wander(float deltaTime)
    {
        //Est-ce que la shell est a l'exterieur des bodure ?
        if (IsOutOfBounds(Position))
        {
            // Shell.pos - Center.pos
            Vector2 deltaToCenter = Game.instance.gameCamera.Center - Position;

            //On set la turnAcc vers le centre de la map
            turnAcc = Vector2.Angle(WorldDirection2D(), deltaToCenter) > 0 ? -maxTurnSpeed : maxTurnSpeed;

            //Turn !
            Rotation = Mathf.MoveTowardsAngle(Rotation, VectorToAngle(deltaToCenter), maxTurnSpeed * deltaTime);
        }
        else
        {
            //Devrais-t-choisir une nouvelle acceleration ?
            if (chooseNewTurnAcc < 0)
            {
                turnAcc = Random.Range(-maxTurnAcceleration, maxTurnAcceleration);
                chooseNewTurnAcc = Random.Range(minPickRate, maxPickRate);
            }
            chooseNewTurnAcc -= deltaTime;

            //Change turn speed
            turnSpeed += turnAcc * deltaTime;
            turnSpeed = Mathf.Clamp(turnSpeed, -maxTurnSpeed, maxTurnSpeed);

            //Turn !
            Rotation = Rotation + (turnSpeed * deltaTime);
        }
    }

    bool IsOutOfBounds(Vector2 pos)
    {
        float rightBorder = Game.instance.gameCamera.ScreenSize.x / 2 - screenBorderWidth;
        float halfHeight = Game.instance.gameCamera.ScreenSize.y / 2 - screenBorderWidth;

        if (pos.x > rightBorder || pos.x < -rightBorder)
            return true;

        if (pos.y > Game.instance.gameCamera.Height + halfHeight || pos.y < Game.instance.gameCamera.Height - halfHeight)
            return true;

        return false;
    }

    Unit FindClosestTargetTo(Vector2 pos)
    {
        List<Unit> units = Game.instance.units;
        Unit closestUnit = null;

        float smallestDistance = float.PositiveInfinity;

        for (int i = 0; i < units.Count; i++)
        {
            //Ignore allies
            if (units[i].allegiance == Allegiance.Ally)
                continue;

            float distance = Vector3.Distance(units[i].Position, pos);

            if (distance < smallestDistance)
            {
                closestUnit = units[i];
                smallestDistance = distance;
            }
        }
        return closestUnit;
    }

    private void ColliderListener_onTriggerEnter(ColliderInfo other, ColliderListener listener)
    {
        Die();
    }

    protected override void Die()
    {
        base.Die();

        rb.velocity = Vector2.zero;
        enabled = false;

        animator.Explode(delegate ()
            {
                gameObject.SetActive(false);
                if (onDeactivate != null)
                    onDeactivate();
            });
    }
}
