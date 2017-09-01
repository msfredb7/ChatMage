using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearmanVehicle : EnemyVehicle
{
    [Header("Spearman")]
    public SpearmanAnimatorV2 animator;
    public Transform deadBody;

    [Header("Attack")]
    public SimpleColliderListener attackListener;
    public float bumpForce = 5;

    [NonSerialized]
    public bool spearAttackConsumed = false;

    protected override void Awake()
    {
        base.Awake();
        attackListener.onTriggerEnter += AttackListener_onTriggerEnter;
    }

    public override int Attacked(ColliderInfo on, int amount, Unit unit, ColliderInfo source = null)
    {
        amount = CheckBuffs_Attacked(on, amount, unit, source);

        if (amount <= 0 && !isDead)
            return 1;

        if (unit != null)
            deadBody.rotation = Quaternion.Euler(Vector3.forward *((Position - unit.Position).ToAngle() - 90));

        Die();
        return 0;
    }

    protected override void Die()
    {
        if (!IsDead)
        {
            canTurn.Lock("dead");
            canMove.Lock("dead");

            animator.DeathAnimation(Destroy);
            GetComponent<AI.SpearmanBrain>().enabled = false;
        }
        base.Die();
    }

    private void AttackListener_onTriggerEnter(ColliderInfo other, ColliderListener listener)
    {
        if (spearAttackConsumed)
            return;

        if (targets.IsValidTarget(other.parentUnit.allegiance))
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
