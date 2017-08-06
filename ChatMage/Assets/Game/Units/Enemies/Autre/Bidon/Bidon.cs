using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bidon : Unit, IAttackable
{
    public Collider2D col;
    public int Attacked(ColliderInfo on, int amount, Unit otherUnit, ColliderInfo source = null)
    {
        Destroy(gameObject);
        return 1;
    }
}
