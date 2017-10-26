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
    public Animator animator;

    public int Attacked(ColliderInfo on, int amount, Unit otherUnit, ColliderInfo source = null)
    {
        hp -= amount;

        if (hp <= 0 && !isDead)
            Die();

        return hp;
    }

    public float GetSmashJuiceReward()
    {
        return 0;
    }

    protected override void Die()
    {
        if (!isDead)
        {
            GetComponent<SoundPlayer>().PlaySound();
            onDeathEvent.Invoke();

            animator.SetTrigger("dead");
            GetComponent<Collider2D>().enabled = false;
        }

        base.Die();
    }
}
