using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherVehicle : EnemyVehicle
{
    [Header("Archer"), Header("Animations")]
    public ArcherAnimatorV2 animator;
    public Transform deadBody;

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

    public event SimpleEvent onAmmoChange;

    private int ammo = 1;
    private bool shooting = false;
    private float shootCooldownRemains;

    public float ShootCooldownRemains { get { return shootCooldownRemains; } }

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
        base.Update();

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
        if (onAmmoChange != null)
            onAmmoChange();
    }

    public void OnShoot()
    {
        shootCooldownRemains = shootCooldown;
        ammo--;
        if (onAmmoChange != null)
            onAmmoChange();
    }

    public override int Attacked(ColliderInfo on, int amount, Unit unit, ColliderInfo source = null)
    {
        amount = CheckBuffs_Attacked(on, amount, unit, source);

        if (amount <= 0 && !IsDead)
            return 1;
        
        if (unit != null)
            deadBody.rotation = Quaternion.Euler(Vector3.forward * ((unit.Position - Position).ToAngle()));

        Die();
        return 0;
    }
    protected override void Die()
    {
        if (IsDead)
            return;

        base.Die();

        canTurn.Lock("dead");
        canMove.Lock("dead");

        animator.DeathAnimation(Destroy);
        GetComponent<AI.ArcherBrain>().enabled = false;
    }
}
