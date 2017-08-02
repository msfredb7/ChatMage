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
    protected Unit target;
    protected Vector2 meToTarget = Vector2.zero;

    private bool noTargetOnThisFrame = false;

    private EnemyBehavior currentBehavior;
    private EnemyBehavior forcedBehavior;

    protected virtual void Start()
    {
        myVehicle.Stop();
        myVehicle.targets.onTargetRemoved += ClearTarget;
    }

    protected virtual void Update()
    {
        if (target == null)
            TryToFindTarget();

        noTargetOnThisFrame = !EvaluateUnit(target);

        if (!noTargetOnThisFrame)
            meToTarget = target.Position - myVehicle.Position;

        if (noTargetOnThisFrame)
            UpdateWithoutTarget();
        else
            UpdateWithTarget();

        if (currentBehavior != null)
            currentBehavior.Update(target, myVehicle.DeltaTime());
    }

    protected void ClearTarget()
    {
        target = null;
    }

    public void TryToFindTarget()
    {
        target = myVehicle.targets.TryToFindTarget(myVehicle);
    }

    private bool EvaluateUnit(Unit unit)
    {
        return unit != null && !unit.IsDead && unit.gameObject.activeSelf && (!(unit is IVisible) || (unit as IVisible).IsVisible());
    }

    protected abstract void UpdateWithTarget();
    protected abstract void UpdateWithoutTarget();
    protected abstract EnemyVehicle myVehicle { get; }

    public bool IsForcedIntoState { get { return forcedBehavior != null; } }

    private void EnterBehavior(EnemyBehavior newBehaviour)
    {
        if (currentBehavior != null)
            currentBehavior.Exit(target);

        currentBehavior = newBehaviour;

        if (newBehaviour != null)
            newBehaviour.Enter(target);
    }

    public bool IsBehavior<T>() where T : EnemyBehavior
    {
        return currentBehavior != null && currentBehavior.GetType() == typeof(T);
    }
    public bool CanGoTo<T>() where T : EnemyBehavior
    {
        return !IsBehavior<T>() && !IsForcedIntoState;
    }

    /// <summary>
    /// On devrait toujours checker CanGoTo() avant. Pour ne pas faire de garbage
    /// </summary>
    public void SetBehavior(EnemyBehavior newBehavior)
    {
        //If forced into another behavior, return
        if (IsForcedIntoState)
            return;

        //Enter behavior
        EnterBehavior(newBehavior);
    }

    public void RemoveForcedBehavior(EnemyBehavior behavior)
    {
        if (forcedBehavior == behavior)
            forcedBehavior = null;
    }

    public bool ForceBehavior(EnemyBehavior newBehavior, bool overridePreviousForcedState = false)
    {
        //Already in another state ? Can it override it ?
        if (IsForcedIntoState && !overridePreviousForcedState)
            return false;

        forcedBehavior = newBehavior;

        //Enter behavior
        EnterBehavior(newBehavior);

        //Set duration
        //forcedInStateDuration = duration;

        return true;
    }
}
