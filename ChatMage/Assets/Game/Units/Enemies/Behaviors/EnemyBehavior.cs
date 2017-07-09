using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class EnemyBehavior<T> : EnemyBehavior where T : EnemyVehicle
{
    protected T vehicle;

    public EnemyBehavior(T vehicle)
    {
        this.vehicle = vehicle;
    }
}
public abstract class EnemyBehavior
{
    public abstract void Enter(Unit target);
    public abstract void Update(Unit target, float deltaTime);
    public abstract void Exit(Unit target);
}