using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Utility;

public class MovingUnit : Unit
{
    [System.NonSerialized]
    public Vector3 speed;
    public Locker canMove = new Locker();
    
    protected Transform tr;

    protected virtual void Start()
    {
        tr = GetComponent<Transform>();
    }

    protected virtual void Update()
    {
        tr.position += speed * DeltaTime();
    }

    public override Vector3 Speed()
    {
        return speed;
    }
    public override Vector3 WorldDirection()
    {
        return speed.normalized;
    }
    public override Vector2 WorldDirection2D()
    {
        return speed.normalized;
    }
}
