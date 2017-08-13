using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestructibleDoor : Unit, IAttackable
{
    [Header("Door")]
    public int hp = 2;
    public UnityEvent onDeathEvent = new UnityEvent();

    public int Attacked(ColliderInfo on, int amount, Unit otherUnit, ColliderInfo source = null)
    {
        hp-= amount;

        if (hp <= 0 && !isDead)
            Die();

        return hp;
    }

    protected override void Die()
    {
        if (!isDead)
            onDeathEvent.Invoke();

        base.Die();

        //Death anim
        Destroy();
    }
}
