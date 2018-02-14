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
    [Forward]
    public Targets targets;

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

        GetComponent<SoundPlayer>().PlaySound();
    }

    private void BlueShellScript_onTimeScaleChange(Unit unit)
    {
        if (GetComponent<TrailRenderer>() != null)
            GetComponent<TrailRenderer>().time = 0.2f / TimeScale;
    }

    public void ResetValues(Vector2 position)
    {
        if (Game.Instance.Player != null)
            Rotation = Game.Instance.Player.vehicle.Rotation;

        gameObject.SetActive(true);
        enabled = true;
        tr.position = position;
        target = null;
        wandering = true;
        wanderingRemains = wanderDuration;
        turnAcc = 0;
        turnSpeed = 0;
        chooseNewTurnAcc = 0;
        isDead = false;
        canMove.Unlock("exl");

        GetComponent<AudioSource>().enabled = true;
        GetComponent<SoundPlayer>().PlaySound();

        animator.ResetValues();
    }

    protected override void Update()
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
            if (!Unit.HasPresence(target))
            {
                target = FindClosestTargetTo(transform.position);
                Wander(FixedDeltaTime());
            }
            else
            {
                Rotation = Mathf.MoveTowardsAngle(Rotation,
                    VectorToAngle(target.Position - Position),
                    maxTurnSpeed * FixedDeltaTime());
            }
        }
    }

    private void Wander(float deltaTime)
    {
        //Est-ce que la shell est a l'exterieur des bodure ?
        if (IsOutOfBounds(Position))
        {
            // Shell.pos - Center.pos
            Vector2 deltaToCenter = Game.Instance.gameCamera.Center - Position;

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
        float rightBorder = Game.Instance.gameCamera.ScreenSize.x / 2 - screenBorderWidth;
        float halfHeight = Game.Instance.gameCamera.ScreenSize.y / 2 - screenBorderWidth;

        if (pos.x > rightBorder || pos.x < -rightBorder)
            return true;

        if (pos.y > Game.Instance.gameCamera.Height + halfHeight || pos.y < Game.Instance.gameCamera.Height - halfHeight)
            return true;

        return false;
    }

    Unit FindClosestTargetTo(Vector2 pos)
    {
        Unit closestUnit = null;
        float smallestDistance = float.PositiveInfinity;

        LinkedListNode<Unit> node = Game.Instance.attackableUnits.First;
        foreach (Unit unit in Game.Instance.attackableUnits)
        {
            //Ignore allies
            if (!targets.IsValidTarget(unit))
                continue;

            float distance = (unit.Position - pos).sqrMagnitude;

            if (distance < smallestDistance)
            {
                closestUnit = unit;
                smallestDistance = distance;
            }
        }

        return closestUnit;
    }

    private void ColliderListener_onTriggerEnter(ColliderInfo other, ColliderListener listener)
    {
        if (!isDead)
            Die();
    }

    protected override void Die()
    {
        base.Die();

        GetComponent<AudioSource>().enabled = false;

        rb.velocity = Vector2.zero;
        enabled = false;
        canMove.Lock("exl");

        animator.Explode(delegate ()
            {
                gameObject.SetActive(false);
                if (onDeactivate != null)
                    onDeactivate();
            });
    }
}
