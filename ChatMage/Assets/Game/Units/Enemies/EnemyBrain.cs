using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using System.Reflection;
using System;



/// <summary>
/// Enemy Brain, with vehicle type T
/// </summary>
/// <typeparam name="T">Vehicle Type</typeparam>
public abstract class EnemyBrain<T> : EnemyBrain where T : EnemyVehicle
{
    protected T vehicle;

    protected override void Awake()
    {
        base.Awake();
        vehicle = GetComponent<T>();
        if (vehicle == null)
            throw new Exception("Could not find vehicle of type: " + typeof(T).ToString());
    }
    protected override EnemyVehicle myVehicle
    {
        get
        {
            return vehicle;
        }
    }
}

public abstract class EnemyBrain : BaseBehavior
{
    protected PlayerController player;
    protected Vector2 meToPlayer = Vector2.zero;

    private bool noPlayerOnThisFrame = false;

    private EnemyBehavior currentBehavior;
    private float forcedInStateDuration = -1;

    protected virtual void Start()
    {
        player = Game.instance.Player;
        myVehicle.Stop();
    }

    void Update()
    {
        if (player == null)
            player = Game.instance.Player;

        noPlayerOnThisFrame = player == null || !player.playerStats.isVisible || player.vehicle.IsDead || !player.gameObject.activeSelf;

        if (!noPlayerOnThisFrame)
            meToPlayer = player.vehicle.Position - myVehicle.Position;

        if (noPlayerOnThisFrame)
            UpdateNoPlayer();
        else
            UpdatePlayer();

        if (currentBehavior != null)
            currentBehavior.Update(player, myVehicle.DeltaTime());

        forcedInStateDuration -= myVehicle.DeltaTime();
    }

    protected abstract void UpdatePlayer();
    protected abstract void UpdateNoPlayer();
    protected abstract EnemyVehicle myVehicle { get; }

    public bool IsForcedIntoState { get { return forcedInStateDuration > 0; } }

    private void EnterBehavior(EnemyBehavior newBehaviour)
    {
        if (currentBehavior != null)
            currentBehavior.Exit(player);

        currentBehavior = newBehaviour;

        if (newBehaviour != null)
            newBehaviour.Enter(player);
    }

    public void SetBehavior(BehaviorType type)
    {
        //Already in that type of state ?
        if (currentBehavior != null && currentBehavior.Type == type)
            return;

        //If forced into another behavior, return
        if (IsForcedIntoState)
            return;

        //New instance
        EnemyBehavior newBehavior = NewBehaviorByType(type);

        //Enter behavior
        EnterBehavior(newBehavior);
    }

    public void ForceBehavior(BehaviorType type, float duration, bool overridePreviousForcedState = false)
    {
        //Already in that type of state ?
        if (currentBehavior != null && currentBehavior.Type == type)
        {
            //Update duration
            forcedInStateDuration = Math.Max(forcedInStateDuration, duration);
            return;
        }

        //Already in another state ? Can it override it ?
        if (IsForcedIntoState && !overridePreviousForcedState)
            return;

        //New instance
        EnemyBehavior newBehavior = NewBehaviorByType(type);

        //Enter behavior
        EnterBehavior(newBehavior);

        //Set duration
        forcedInStateDuration = duration;
    }

    public BehaviorType CurrentBehaviorType
    {
        get
        {
            if (currentBehavior != null)
                return currentBehavior.Type;
            return BehaviorType.Null;
        }
    }

    private EnemyBehavior NewBehaviorByType(BehaviorType type)
    {
        switch (type)
        {
            default:
            case BehaviorType.Null:
                return null;
            case BehaviorType.Idle:
                return NewIdleBehavior();
            case BehaviorType.Flee:
                return NewFleeBehavior();
            case BehaviorType.Follow:
                return NewFollowBehavior();
            case BehaviorType.Panic:
                return NewPanicBehaviour();
            case BehaviorType.Wander:
                return NewWanderBehavior();
            case BehaviorType.LookPlayer:
                return NewLookPlayerBehavior();
        }
    }

    protected virtual EnemyBehavior NewFleeBehavior()
    {
        return new FleeBehavior(myVehicle);
    }

    protected virtual EnemyBehavior NewFollowBehavior()
    {
        return new FollowBehavior(myVehicle);
    }

    protected virtual EnemyBehavior NewPanicBehaviour()
    {
        return new PanicBehavior(myVehicle);
    }

    protected virtual EnemyBehavior NewWanderBehavior()
    {
        return new WanderBehavior(myVehicle);
    }

    protected virtual EnemyBehavior NewIdleBehavior()
    {
        return new IdleBehavior(myVehicle);
    }

    protected virtual EnemyBehavior NewLookPlayerBehavior()
    {
        return new LookPlayerBehavior(myVehicle);
    }
}
