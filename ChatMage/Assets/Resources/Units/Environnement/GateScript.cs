using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScript : Unit, IAttackable
{
    bool done = false;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (Game.instance != null && !done)
        {
            done = true;
            Game.instance.AddExistingUnit(this);
        }
    }

    public int Attacked(ColliderInfo on, int amount, Unit otherUnit, ColliderInfo source = null)
    {
        Game.instance.units.Remove(this);
        Game.instance.gameCamera.followPlayer = true;
        Destroy(gameObject);
        return 0;
    }
}
