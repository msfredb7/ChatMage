using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Utility;

public abstract class MovingUnit : Unit
{
    public override Vector3 WorldDirection()
    {
        return Speed.normalized;
    }
    public override Vector2 WorldDirection2D()
    {
        return Speed.normalized;
    }
}
