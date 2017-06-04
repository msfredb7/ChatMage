using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TirRocheVehicle : EnemyVehicle
{
    [Header("Tir Roche")]
    public float walkSpeed = 2;
    public float fleeSpeed = 4;
    public int maxAmmo = 1;

    private int ammo = 0;

    public void WalkMode()
    {
        MoveSpeed = walkSpeed;
    }
    public void RunMode()
    {
        MoveSpeed = fleeSpeed;
    }

    public void Shoot()
    {
        if (ammo <= 0)
            return;

        ammo--;
    }

    public int Ammo { get { return ammo; } }

    public void Reload(Action onComplete)
    {
        if (ammo == maxAmmo)
            return;

        ammo++;
        if (onComplete != null)
            onComplete();
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
