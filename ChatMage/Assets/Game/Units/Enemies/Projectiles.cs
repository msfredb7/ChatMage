using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Projectiles : MovingUnit
{
    [NonSerialized]
    public Unit origin;

    public void Init(Unit origin)
    {
        this.origin = origin;

        targets = new List<Allegiance>(origin.targets);
    }
}
