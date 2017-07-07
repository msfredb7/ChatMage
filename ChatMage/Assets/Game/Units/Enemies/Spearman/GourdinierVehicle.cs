using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GourdinierVehicle : EnemyVehicle
{
    [Header("Gourdinier")]
    public float attackCooldown = 3;
    public GourdinierAnimator animator;
    public SpriteRenderer bodySprite;

    [System.NonSerialized]
    public bool isAttacking;

    private float currentCooldown = 0;

    void Update()
    {
        if (currentCooldown > 0)
            currentCooldown -= DeltaTime();
    }

    public bool CanAttack()
    {
        return currentCooldown <= 0 && !isAttacking;
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

    public void Attack()
    {
        isAttacking = true;
        animator.Charge(OnChargeComplete);
    }

    void OnChargeComplete()
    {
        animator.Attack(OnCompleteAttack);
        //attackAnim.
    }

    void OnCompleteAttack()
    {
        isAttacking = false;
        currentCooldown = attackCooldown;
    }
}
