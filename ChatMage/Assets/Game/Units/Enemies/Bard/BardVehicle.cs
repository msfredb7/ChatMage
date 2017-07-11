using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BardVehicle : EnemyVehicle
{
    public override int Attacked(ColliderInfo on, int amount, Unit unit, ColliderInfo source = null)
    {
        if (amount <= 0 && !IsDead)
            return 1;

        Die();
        return 0;
    }

    protected override void Die()
    {
        base.Die();

        //Death animation
        Destroy();
    }
}
