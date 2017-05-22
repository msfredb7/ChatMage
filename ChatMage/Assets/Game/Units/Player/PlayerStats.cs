using CCC.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class PlayerStats : PlayerComponent
{
    
    [System.NonSerialized]
    public StatInt health = new StatInt(3,0,3,BoundMode.Cap);
    [System.NonSerialized]
    public StatInt armor = new StatInt(0, 0, 0, BoundMode.Cap);
    [System.NonSerialized]
    public StatInt frontDamage = new StatInt(1, 1, 1, BoundMode.Cap);
    public bool damagable;
    public bool isVisible = true; // TODO

    public UnityEvent onDeath = new UnityEvent();
    public UnityEvent onHit = new UnityEvent();
    public UnityEvent onRegen = new UnityEvent();

    public bool isNotDead = true;

    public void Hit()
    {
        if (damagable)
        {
            if (armor > 0)
                armor--;
            else
                health--;
            onHit.Invoke();
        }
        if (health <= 0)
            Death();
    }

    public void Hit(int amount)
    {
        Debug.Log("Player has been hit!");
        int trackingAmount = 0;
        if (damagable)
        {
            if (armor > 0)
            {
                armor.Set(armor-amount);
                if (armor < 0)
                {
                    trackingAmount = armor * -1;
                    armor.Set(0);
                    health.Set(health - trackingAmount);
                }
            }
            else
                health.Set(health - amount);
            onHit.Invoke();
        }
        if (health <= 0)
            Death();
    }

    public override void OnGameReady()
    {
    }

    public override void OnGameStarted()
    {
    }

    public void Regen()
    {
        Debug.Log("Player regeneration!");
        health++;
        onRegen.Invoke();
    }

    public void Regen(int amount)
    {
        Debug.Log("Player regeneration!");
        health.Set(health + amount);
        onRegen.Invoke();
    }

    void Death()
    {
        if (isNotDead)
        {
            isNotDead = false;
            onDeath.Invoke();
            Destroy(gameObject);
        }
    }
}
