using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleMageBrain : EnemyBrain<BubbleMageVehicle>
{
    public Targets bubbleTargets;
    public bool fleePlayer = false;
    public float fleeDistance = 3;
    public float shootDistance = 4;
    [Header("Devrais etre plus long que la duree de voyage de la bubble")]
    public float shootPause = 2;
    public BubbleProjectile projectilePrefab;

    private float fleeDistSQR;
    private float shootDistSQR;
    private Unit friendTarget;
    private bool isShooting = false;
    private float shootPauseRemains = 0;

    protected override void Awake()
    {
        base.Awake();

        fleeDistSQR = fleeDistance * fleeDistance;
        shootDistSQR = shootDistance * shootDistance;
    }


    protected override void UpdateWithoutTarget()
    {
        //Bubble yo m8ts
        if (isShooting)
        {
            //Is shooting ! Aim at ally
            if (friendTarget != null)
            {
                Vector2 v = friendTarget.Position - vehicle.Position;
                vehicle.TurnToDirection(v.ToAngle(), vehicle.DeltaTime());
            }
        }
        else if (friendTarget != null && !friendTarget.IsDead)
        {
            //Ally in sight, shoot or get closer ?
            Vector2 v = friendTarget.Position - vehicle.Position;

            if (IsBehavior<TweenBehavior>())
            {
                //We're charging! Look at target
                vehicle.TurnToDirection(v.ToAngle(), vehicle.DeltaTime());
                vehicle.Stop();
            }
            else if (v.sqrMagnitude > shootDistSQR)
            {
                //Get closer to ally
                vehicle.GotoPosition(friendTarget.Position);
                SetBehavior(null);
            }
            else
            {
                //Start Charge !
                if (!IsForcedIntoState && !IsBehavior<TweenBehavior>())
                {
                    vehicle.Stop();
                    TweenBehavior chargeBehavior = new TweenBehavior(vehicle, vehicle.animator.Charge());
                    chargeBehavior.onComplete = OnChargeComplete;
                    SetBehavior(chargeBehavior);
                }
            }
        }
        else
        {
            //Search for allies
            if (shootPauseRemains <= 0)
                friendTarget = SearchForUnBubbledEnemy();
            if (CanGoTo<WanderBehavior>())
                SetBehavior(new WanderBehavior(vehicle));
        }
    }

    protected override void Update()
    {
        base.Update();

        if (shootPauseRemains > 0)
            shootPauseRemains -= vehicle.DeltaTime();
    }

    protected override void UpdateWithTarget()
    {
        if (fleePlayer && !isShooting && meToTarget.sqrMagnitude < fleeDistSQR)
        {
            if (CanGoTo<FleeBehavior>())
                SetBehavior(new FleeBehavior(vehicle));
        }
        else
        {
            UpdateWithoutTarget();
        }
    }

    void OnStopShooting()
    {
        isShooting = false;
    }

    private void OnChargeComplete()
    {
        TweenBehavior shootBehavior = new TweenBehavior(vehicle, vehicle.animator.Shoot(ShootBubble));
        shootBehavior.onCancel = OnStopShooting;
        shootBehavior.onComplete = OnStopShooting;

        SetBehavior(shootBehavior);
        isShooting = true;
    }

    private void ShootBubble()
    {
        Game.instance.SpawnUnit(projectilePrefab, vehicle.Position)
            .Init(vehicle.Rotation, vehicle, friendTarget.transform);

        shootPauseRemains = shootPause;
        friendTarget = null;
    }

    private Unit SearchForUnBubbledEnemy()
    {
        if (Game.instance == null)
            return null;

        Unit recordHolder = null;
        float record = float.PositiveInfinity;

        Vector2 myPos = vehicle.Position;
        foreach (Unit unit in Game.instance.attackableUnits)
        {
            if (!unit.IsDead && bubbleTargets.IsValidTarget(unit) && !unit.HasBuffOfType(typeof(BubbleBuff)))
            {
                Vector2 v = unit.Position - myPos;
                float dist = v.sqrMagnitude;
                if (dist < record)
                {
                    record = dist;
                    recordHolder = unit;
                }
            }
        }
        return recordHolder;
    }
}
