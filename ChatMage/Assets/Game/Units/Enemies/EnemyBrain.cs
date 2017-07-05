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
    protected Vector2 meToTarger = Vector2.zero;

    private bool noTargetOnThisFrame = false;

    private EnemyBehavior currentBehavior;
    private float forcedInStateDuration = -1;

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
            meToTarger = target.Position - myVehicle.Position;

        if (noTargetOnThisFrame)
            UpdateWithoutTarget();
        else
            UpdateWithTarget();

        if (currentBehavior != null)
            currentBehavior.Update(target, myVehicle.DeltaTime());

        forcedInStateDuration -= myVehicle.DeltaTime();
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
            PlayerController player = Game.instance.Player;
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
                    if(attackable != null)
                    {
                        float sqrDistance = (unit.Position - myPos).sqrMagnitude;
                        if(sqrDistance < smallestDistance)
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

    public bool IsForcedIntoState { get { return forcedInStateDuration > 0; } }

    private void EnterBehavior(EnemyBehavior newBehaviour)
    {
        if (currentBehavior != null)
            currentBehavior.Exit(target);

        currentBehavior = newBehaviour;

        if (newBehaviour != null)
            newBehaviour.Enter(target);
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
