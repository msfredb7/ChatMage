using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScript : Unit, IAttackable
{

    public int Attacked(ColliderInfo on, int amount, Unit otherUnit, ColliderInfo source = null)
    {
        Game.instance.units.Remove(this);
        Game.instance.gameCamera.followPlayer = true;
        Destroy(gameObject);
        return 0;
    }
}
