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
    public float shootCooldown = 1.5f;
    public TirRocheAnimator animator;

    private int ammo = 0;
    private bool shooting = false;
    private float shootCooldownRemains = -1;

    public void WalkMode()
    {
        MoveSpeed = walkSpeed;
    }
    public void RunMode()
    {
        MoveSpeed = fleeSpeed;
    }

    void Update()
    {
        if (shootCooldownRemains > 0)
            shootCooldownRemains -= DeltaTime();
    }

    public bool IsShooting { get { return shooting; } }
    public bool CanShoot { get { return !shooting && shootCooldownRemains < 0; } }

    public void Shoot(Unit target)
    {
        if (IsShooting || ammo <= 0)
            return;

        animator.Shoot(target,
            delegate ()
        {
            shootCooldownRemains = shootCooldown;
            shooting = false;
        });
        shooting = true;

        ammo--;
    }

    public int Ammo { get { return ammo; } }

    public void Reload(Action onComplete)
    {
        if (ammo == maxAmmo)
            return;

        EngineOff();

        ammo++;

        // TODO : Animation de recharge de l'archer
        animator.Reload(ammo, maxAmmo, onComplete);
    }

    public override int Attacked(ColliderInfo on, int amount, Unit unit, ColliderInfo source = null)
    {
        if (amount <= 0)
            return 1;

        Die();
        return 0;
    }
    protected override void Die()
    {
        if (IsDead)
            return;

        base.Die();

        //Death anim
        Destroy();
    }
}
