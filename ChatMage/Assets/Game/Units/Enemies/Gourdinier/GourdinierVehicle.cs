using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GourdinierVehicle : EnemyVehicle
{
    [Header("Gourdinier")]
    public float attackCooldown = 3;
    public SpriteRenderer bodySprite;

    [System.NonSerialized]
    public bool isAttacking;

    private float currentCooldown = 0;
    private GourdinierAttackAnim attackAnim;

    void Update()
    {
        if (currentCooldown > 0)
            currentCooldown -= DeltaTime();
    }

    public bool CanAttack()
    {
        return currentCooldown <= 0 && !isAttacking;
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

        Destroy(gameObject);
    }

    public void Attack()
    {
        isAttacking = true;
        attackAnim = new GourdinierAttackAnim(this);
        attackAnim.Charge(OnChargeComplete);
    }

    void OnChargeComplete()
    {
        //attackAnim.
    }

    void OnCompleteAttack()
    {
        isAttacking = false;
        currentCooldown = attackCooldown;
    }
}
