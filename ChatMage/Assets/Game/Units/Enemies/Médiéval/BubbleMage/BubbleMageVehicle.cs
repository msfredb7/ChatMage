using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleMageVehicle : EnemyVehicle
{
    public BubbleMageAnimator animator;

    public override int Attacked(ColliderInfo on, int amount, Unit unit, ColliderInfo source = null)
    {
        amount = CheckBuffs_Attacked(on, amount, unit, source);

        if (amount <= 0 && !isDead)
            return 1;

        if (!isDead)
            Die();

        return 0;
    }

    protected override void Die()
    {
        base.Die();

        Destroy();
    }
}
