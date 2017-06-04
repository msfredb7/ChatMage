using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TirRocheVehicle : EnemyVehicle
{
    [Header("Tir Roche")]
    public float walkSpeed = 2;
    public float fleeSpeed = 4;

    public void WalkMode()
    {
        MoveSpeed = walkSpeed;
    }
    public void RunMode()
    {
        MoveSpeed = fleeSpeed;
    }
    public override int Attacked(ColliderInfo on, int amount, MonoBehaviour source)
    {
        if (amount <= 0)
            return 1;

        Die();
        return 0;
    }
    protected override void Die()
    {
        base.Die();

        //Death anim
        Destroy(gameObject);
    }
}
