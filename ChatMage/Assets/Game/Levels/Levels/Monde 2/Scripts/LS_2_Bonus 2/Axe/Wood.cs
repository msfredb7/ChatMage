using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Wood : Unit, IAttackable
{
    [Header("Linking"), Forward]
    public Targets targets;
    public int smashJuice = 0;

    void Start()
    {
        // ANIMATION D'APPARITION DU BOIS
        // ICI...
    }

    //Si le shield se fait frapper, il resiste et tourne
    public int Attacked(ColliderInfo on, int amount, Unit otherUnit, ColliderInfo source = null)
    {
        if (otherUnit is CuttingAxe)
        {
            // ANIMATION DE DESTRUCTIONS OU DE COUPAGE DU BOIS 
            // ICI...

            if (!IsDead)
                Die();
            return 0;
        }
        else
        {
            return 1;
        }
    }

    public float GetSmashJuiceReward()
    {
        return smashJuice;
    }

    protected override void Die()
    {
        base.Die();

        Destroy();
    }
}
