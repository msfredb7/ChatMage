using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScript : Unit, IAttackable
{

    public int Attacked(ColliderInfo on, int amount, Unit otherUnit, ColliderInfo source = null)
    {
        Die();
        return 0;
    }

    protected override void Die()
    {
        Game.instance.gameCamera.followPlayer = true;

        base.Die();

        Destroy();
    }
}
