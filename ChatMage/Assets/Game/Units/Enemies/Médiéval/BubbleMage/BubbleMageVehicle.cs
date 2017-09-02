using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleMageVehicle : EnemyVehicle
{
    public BubbleMageAnimatorV2 animator;
    public Transform deadBody;

    public override int Attacked(ColliderInfo on, int amount, Unit unit, ColliderInfo source = null)
    {
        amount = CheckBuffs_Attacked(on, amount, unit, source);

        if (amount <= 0 && !isDead)
            return 1;

        if (unit != null)
            deadBody.rotation = Quaternion.Euler(Vector3.forward * ((Position - unit.Position).ToAngle() - 90));

        if (!isDead)
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
            GetComponent<AI.BubbleMageBrain>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }

        base.Die();
    }
}
