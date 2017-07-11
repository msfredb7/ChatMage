using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherVehicle : EnemyVehicle
{
    [Header("Archer"), Header("Animations")]
    public ArcherAnimator animator;

    [Header("Walk")]
    public float walkMoveSpeed = 2;
    public float walkTurnSpeed = 200;
    [Header("Fleed")]
    public float fleeMoveSpeed = 4;
    public float fleeTurnSpeed = 360;

    [Header("Shoot")]
    public float shootCooldown;
    public ArcherArrow arrowPrefab;
    public Transform arrowLaunchLocation;

    private int ammo = 1;
    private bool shooting = false;
    private float shootCooldownRemains;

    public void WalkMode()
    {
        MoveSpeed = walkMoveSpeed;
        turnSpeed = walkTurnSpeed;
    }
    public void FleeMode()
    {
        MoveSpeed = fleeMoveSpeed;
        turnSpeed = fleeTurnSpeed;
    }

    protected override void Update()
    {
        if (shootCooldownRemains > 0)
            shootCooldownRemains -= DeltaTime();
    }

    public bool IsShooting
    {
        get { return shooting; }
        set { shooting = value; }
    }
    public bool CanShoot { get { return !shooting && ammo > 0 && shootCooldownRemains <= 0; } }

    public int Ammo { get { return ammo; } }

    public void GainAmmo()
    {
        ammo++;
    }

    public void LoseAmmo()
    {
        shootCooldownRemains = shootCooldown;
        ammo--;
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
