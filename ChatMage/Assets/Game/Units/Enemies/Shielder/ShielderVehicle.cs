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
    public float bumpDuration = 0f;
    public float passiveMoveSpeed;
    public float passiveTurnSpeed;

    public Unit_Event onShieldPhysicalHit;

    private float battleMoveSpeed;
    private float battleTurnSpeed;
    private bool battleMode = true;

    protected override void Awake()
    {
        base.Awake();
        battleMoveSpeed = MoveSpeed;
        battleTurnSpeed = turnSpeed;
        swordListener.OnTriggerEnter += OnSwordHit;
    }

    public void OnSwordHit(ColliderInfo other, ColliderListener listener)
    {
        if (other.parentUnit.allegiance == Allegiance.Ally)
        {
            IAttackable attackable = other.parentUnit.GetComponent<IAttackable>();
            if (attackable != null)
                attackable.Attacked(other, 1, this, listener.info);
        }
    }

    public override int Attacked(ColliderInfo on, int amount, Unit unit, ColliderInfo source = null)
    {
        amount = CheckBuffs_Attacked(on, amount, unit, source);

        if (amount <= 0 && !isDead)
            return 1;

        if (on.GroupParent == shieldGroup)
        {
            //Attacked on shield
            if (onShieldPhysicalHit != null && source != null)
            {
                if (targets.IsValidTarget(unit.allegiance) && unit is Vehicle)
                {
                    //Bump !
                    (unit as Vehicle).Bump((unit.Position - Position).normalized * bumpStrength, bumpDuration, BumpMode.VelocityAdd);
                }
                onShieldPhysicalHit(unit);
            }
            return 1;
        }
        else
        {
            //Attacked behind !
            Die();

            return 0;
        }
    }
    public void PassiveMode()
    {
        if (!battleMode)
            return;
        battleMode = false;
        MoveSpeed = passiveMoveSpeed;
        turnSpeed = passiveTurnSpeed;
    }
    public void BattleMode()
    {
        if (battleMode)
            return;
        battleMode = true;
        MoveSpeed = battleMoveSpeed;
        turnSpeed = battleTurnSpeed;
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
