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
    //private float forcedInStateDuration = -1;
    private EnemyBehavior forcedBehavior;

    protected virtual void Start()
    {
        myVehicle.Stop();
        myVehicle.onRemoveTarget += ClearTarget;
    }

    void Update()
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

        //forcedInStateDuration -= myVehicle.DeltaTime();
    }

    protected void ClearTarget()
    {
        target = null;
    }

    public void TryToFindTarget()
    {
        if (myVehicle.targets == null || myVehicle.targets.Count == 0)
            return;

        //En g�n�ral, on passe ici, sachant que les ennemi cherche pas mal toujours le joueur
        if (myVehicle.targets.Count == 1 && myVehicle.targets[0] == Allegiance.Ally)
        {
            PlayerController player = Game.instance == null ? null : Game.instance.Player;
            if (player != null)
            {
                if (EvaluateUnit(player.vehicle))
                {
                    target = player.vehicle;
                    return;
                }
            }
        }
        else
        {
            //Cherche a travers tous les units pour trouver la plus pret
            //Yaurait moyen d'optimiser ca si on fait des listes de units plus pr�cise dans Game
            //  ex: une liste d'IAttackable

            List<Unit> allUnits = Game.instance.units;

            Vector2 myPos = myVehicle.Position;
            float smallestDistance = float.PositiveInfinity;
            Unit recordHolder = null;
            for (int i = 0; i < allUnits.Count; i++)
            {
                Unit unit = allUnits[i];
                if (unit == myVehicle)
                    continue;
                if (myVehicle.IsValidTarget(unit.allegiance))
                {
                    IAttackable attackable = unit.GetComponent<IAttackable>();
                    if (attackable != null)
                    {
                        float sqrDistance = (unit.Position - myPos).sqrMagnitude;
                        if (sqrDistance < smallestDistance)
                        {
                            smallestDistance = sqrDistance;
                            recordHolder = unit;
                        }
                    }
                }
            }
            target = recordHolder;
        }
    }

    private bool EvaluateUnit(Unit unit)
    {
        return unit != null && unit.isVisible && !unit.IsDead && unit.gameObject.activeSelf;
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

    //public void UpdateForcedDuration(float duration)
    //{
    //    forcedInStateDuration = Mathf.Max(duration, forcedInStateDuration);
    //}

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
