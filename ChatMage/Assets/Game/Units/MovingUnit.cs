using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Utility;

public class MovingUnit : Unit
{
    [System.NonSerialized]
    public Locker canMove = new Locker();

    public MovingPlatform movingPlatform;

    public Vector2 Speed
    {
        get { return rb.velocity; }
        set { rb.velocity = value; }
    }

    protected Vector2 bounds = new Vector2(10, 10);

    protected virtual void FixedUpdate()
    {
        if (!canMove)
            Speed = Vector2.zero;

        if (movingPlatform != null)
            tr.position += Vector3.up * movingPlatform.GetVerticalSpeed() * Time.fixedDeltaTime;
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
