using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAnimator : MonoBehaviour
{
    public Animator controller;

    protected abstract EnemyVehicle Vehicle
    {
        get;
    }

    protected virtual void Start()
    {
        Vehicle_onTimeScaleChange(Vehicle);
        Vehicle.onTimeScaleChange += Vehicle_onTimeScaleChange; ;
    }

    protected void Vehicle_onTimeScaleChange(Unit unit)
    {
        controller.speed = unit.TimeScale;
    }
}
