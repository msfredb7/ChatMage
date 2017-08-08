using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Wood : Unit, IAttackable
{
    [Header("Linking"), Forward]
    public Targets targets;
    public SimpleColliderListener woodCollider;

    void Start()
    {
        // ANIMATION D'APPARITION DU BOIS
        // ICI...
    }

    //Si le shield se fait frapper, il resiste et tourne
    public int Attacked(ColliderInfo on, int amount, Unit otherUnit, ColliderInfo source = null)
    {
        if(otherUnit is CuttingAxe)
        {
            // ANIMATION DE DESTRUCTIONS OU DE COUPAGE DU BOIS 
            // ICI...

            ForceDie();
            Destroy();
            return 1;
        } else
        {
            return 0;
        }
    }
}
