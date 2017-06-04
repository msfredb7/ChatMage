using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BehaviorType { Null = -1, Idle = 0, Flee = 1, Follow = 2, Panic = 3, Wander = 4, LookPlayer = 5 } //On peut en ajouter

public abstract class EnemyBehavior
{
    protected EnemyVehicle vehicle;

    public EnemyBehavior(EnemyVehicle vehicle)
    {
        this.vehicle = vehicle;
    }
    public abstract void Enter(PlayerController player);
    public abstract void Update(PlayerController player, float deltaTime);
    public abstract void Exit(PlayerController player);
    public abstract BehaviorType Type { get; }
}
