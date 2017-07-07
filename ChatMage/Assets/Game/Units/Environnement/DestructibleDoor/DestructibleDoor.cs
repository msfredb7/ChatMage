using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleDoor : Unit, IAttackable
{
    [Header("Door")]
    public int hp = 2;

    public int Attacked(ColliderInfo on, int amount, Unit otherUnit, ColliderInfo source = null)
    {
        hp-= amount;

        if (hp <= 0 && !isDead)
            Die();

        return hp;
    }

    protected override void Die()
    {
        base.Die();

        //Death anim
        Destroy();
    }
}
