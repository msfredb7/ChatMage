using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BardBrain : EnemyBrain<BardVehicle>
{
    public float singEvery = 6;
    public float safeDistance = 3;

    private float singTimer;

    protected override void Start()
    {
        base.Start();

        ResetSingTimer();
    }

    protected override void UpdateWithoutTarget()
    {
        if (CanGoTo<WanderBehavior>())
            SetBehavior(new WanderBehavior(vehicle));
    }

    protected override void UpdateWithTarget()
    {
        if(singTimer < 0)
        {
            if (CanGoTo<BardSingBehavior>())
                SetBehavior(new BardSingBehavior(vehicle, ResetSingTimer));
        }
        else
        {
            if (CanGoTo<SafeWander>())
                SetBehavior(new SafeWander(vehicle, safeDistance));
        }

        singTimer -= vehicle.DeltaTime();
    }

    private void ResetSingTimer()
    {
        singTimer = singEvery;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 0, 1, 0.2f);
        Gizmos.DrawSphere(transform.position, safeDistance);
    }
}
