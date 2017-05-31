using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Utility;

public abstract class MovingUnit : Unit
{
    [System.NonSerialized]
    public Locker canMove = new Locker();

    protected override void FixedUpdate()
    {
        if (!canMove)
            Speed = Vector2.zero;

        base.FixedUpdate();
    }

    public override Vector3 WorldDirection()
    {
        return Speed.normalized;
    }
    public override Vector2 WorldDirection2D()
    {
        return Speed.normalized;
    }
}
