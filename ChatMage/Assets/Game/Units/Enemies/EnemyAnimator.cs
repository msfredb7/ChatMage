using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAnimator : MonoBehaviour
{
    public Animator controller;

    protected bool moving = false;
    protected int movingHash = Animator.StringToHash("moving");

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

    protected void Update()
    {
        if (Vehicle.Speed.sqrMagnitude < 0.1f)
        {
            if (moving)
            {
                moving = false;
                controller.SetBool(movingHash, false);
            }
        }
        else
        {
            if (!moving)
            {
                moving = true;
                controller.SetBool(movingHash, true);
            }
        }
    }
}
