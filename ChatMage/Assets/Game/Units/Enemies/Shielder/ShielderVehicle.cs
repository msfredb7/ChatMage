using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShielderVehicle : EnemyVehicle
{
    [Header("Shielder Linking")]
    public GameObject shieldGroup;
    public SimpleColliderListener swordListener;
    public ShielderAnimator animator;

    [Header("Shielder Settings")]
    public float bumpStrength;
    public float bumpDuration = 0.2f;
    public float passiveMoveSpeed;
    public float passiveTurnSpeed;

    public Unit_Event onShieldPhysicalHit;

    public bool onlyKillableBySmash;

    private float battleMoveSpeed;
    private float battleTurnSpeed;
    private bool battleMode = true;
    private bool hasHit = false;     //Utilis� pour s'assurer qu'on ne double-tap pas les chose avec l'�p�e

    protected override void Awake()
    {
        base.Awake();
        battleMoveSpeed = MoveSpeed;
        battleTurnSpeed = turnSpeed;
        swordListener.OnTriggerEnter += OnSwordHit;
    }

    public void OnSwordHit(ColliderInfo other, ColliderListener listener)
    {
        //Utilis� pour s'assurer qu'on ne double-tap pas les chose avec l'�p�e
        if (hasHit)
            return;
        hasHit = true;

        if(other.parentUnit.allegiance == Allegiance.Ally)
        {
            IAttackable attackable = other.parentUnit.GetComponent<IAttackable>();
            if (attackable != null)
                attackable.Attacked(other, 1, this, listener.info);
        }
    }

    public override int Attacked(ColliderInfo on, int amount, Unit unit, ColliderInfo source = null)
    {
        if (onlyKillableBySmash) // A ENLEVER
        {
            if(source !=null && source.gameObject.tag == "AC130 Bullet")
                Die();
            return 0;
        }

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
                (on as Vehicle).Bump((on.Position - Position).normalized * bumpStrength, bumpDuration, BumpMode.VelocityAdd);
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
        hasHit = false;
        animator.Attack();
    }

    public bool CanBumpShield { get { return !animator.IsBumping && !animator.IsAttacking; } }

    public void PassiveMode()
    {
        if (!battleMode)
            return;
        battleMode = false;
        MoveSpeed = passiveMoveSpeed;
        turnSpeed = passiveTurnSpeed;
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
        Destroy();
    }
}
