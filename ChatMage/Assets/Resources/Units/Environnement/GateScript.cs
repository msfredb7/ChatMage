﻿using System;
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
            Debug.Log("Adding gate");
            Game.instance.units.Add(this as Unit);
        }
    }

    public int Attacked(ColliderInfo on, int amount, Unit otherUnit, ColliderInfo source = null)
    {
        Game.instance.units.Remove(this);
        Destroy(gameObject);
        return 0;
    }
}
