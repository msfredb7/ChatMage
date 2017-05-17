using CCC.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviour {
    
    public StatInt health;
    public StatInt armor;
    public StatInt frontDamage;
    public bool damagable;

    public UnityEvent onDeath = new UnityEvent();
    public UnityEvent onHit = new UnityEvent();

    public void Init()
    {
        //health = healthLimit;
    }

    public void Hit()
    {
        Debug.Log("Player has been hit!");
        if (damagable)
        {
            onHit.Invoke();
            if (armor > 0)
                armor--;
            else
                health--;
        }
        if (health <= 0)
            onDeath.Invoke();
    }

    public void Regen()
    {
        Debug.Log("Player regeneration!");
        health++;
    }
}
