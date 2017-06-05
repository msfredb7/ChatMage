using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShielderVehicle : EnemyVehicle
{
    [Header("Shielder")]
    public GameObject shieldGroup;
    public ShielderAnimator animator;
    public Unit_Event onShieldPhysicalHit;
    public float bumpStrength;
    public float passiveMoveSpeed;
    public float passiveTurnSpeed;

    private float battleMoveSpeed;
    private float battleTurnSpeed;
    private bool battleMode = true;

    protected override void Awake()
    {
        base.Awake();
        battleMoveSpeed = MoveSpeed;
        battleTurnSpeed = turnSpeed;
    }

    public override int Attacked(ColliderInfo on, int amount, Unit unit, ColliderInfo source = null)
    {
        if (on.groupParent == shieldGroup)
        {
            //Attacked on shield
            if (onShieldPhysicalHit != null && source != null)
                onShieldPhysicalHit(unit);
            return 1;
        }
        else
        {
            //Attacked behind !
            Die();

            return 0;
        }
    }

    public void BumpShield(Unit on, Action callback, float callbackDelay = 0)
    {
        if(on != null)
        {
            if(on is Vehicle)
            {
                (on as Vehicle).Bump((on.Position - Position).normalized * bumpStrength, -1, BumpMode.VelocityAdd);
            }
            else
            {
                on.Speed += (on.Position - Position).normalized * bumpStrength;
            }
        }

        animator.BumpShield(callback, callbackDelay);
    }

    public void Attack()
    {
        animator.Attack();
    }

    public bool CanBumpShield { get { return !animator.IsBumping && !animator.IsAttacking; } }

    public void PassiveMode()
    {
        if (!battleMode)
            return;
        battleMode = false;
        MoveSpeed = passiveMoveSpeed;
        turnSpeed = passiveMoveSpeed;
        animator.HideShield();
    }
    public void BattleMode()
    {
        if (battleMode)
            return;
        battleMode = true;
        MoveSpeed = battleMoveSpeed;
        turnSpeed = battleTurnSpeed;
        animator.BringOutShield();
    }

    protected override void Die()
    {
        base.Die();

        //Disable shield colliders so player doesnt hit them
        foreach (Collider2D collider in shieldGroup.GetComponentsInChildren<Collider2D>())
        {
            collider.enabled = false;
        }

        //Death anim
        Destroy(gameObject);
    }
}
