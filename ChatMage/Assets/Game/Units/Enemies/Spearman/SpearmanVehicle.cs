using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearmanVehicle : EnemyVehicle
{
    [Header("Spearman")]
    public float attackCooldown = 3;
    public SpearmanAnimator animator;
    public SpriteRenderer bodySprite;

    [Header("Attack")]
    public SimpleColliderListener attackListener;
    public float bumpForce = 5;

    private bool isAttacking = false;
    public bool IsAttacking { get { return isAttacking; } }

    private float currentCooldown = 0;
    private bool spearAttackConsumed = false;

    protected override void Awake()
    {
        base.Awake();
        attackListener.onTriggerEnter += AttackListener_onTriggerEnter;
    }

    protected override void Update()
    {
        if (currentCooldown > 0)
            currentCooldown -= DeltaTime();
    }

    public bool CanAttack
    {
        get
        {
            return currentCooldown <= 0 && !isAttacking;
        }
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
        base.Die();

        Destroy();
    }

    public void AttackStarted()
    {
        isAttacking = true;
        spearAttackConsumed = false;
    }

    public void AttackCompleted()
    {
        currentCooldown = attackCooldown;
        isAttacking = false;
    }

    private void AttackListener_onTriggerEnter(ColliderInfo other, ColliderListener listener)
    {
        if (spearAttackConsumed)
            return;

        if (IsValidTarget(other.parentUnit.allegiance))
        {
            IAttackable attackable = other.parentUnit.GetComponent<IAttackable>();
            if (attackable != null)
            {
                spearAttackConsumed = true;
                attackable.Attacked(other, 1, this, listener.info);

                //Bump
                if (other.parentUnit is Vehicle)
                {
                    Vector2 v = other.parentUnit.Position - Position;
                    (other.parentUnit as Vehicle).Bump(v.normalized * bumpForce, -1, BumpMode.VelocityAdd);
                }
            }
        }
    }
}
