using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Utility;

public class MovingUnit : Unit
{
    [System.NonSerialized]
    public Locker canMove = new Locker();

    protected Vector2 bounds = new Vector2(10, 10);
    protected float boundsWidth = 0;
    protected bool useBounds = false;

    protected override void FixedUpdate()
    {
        if (!canMove)
            Speed = Vector2.zero;

        base.FixedUpdate();

        if (useBounds)
            RestrainToBounds();
    }

    void RestrainToBounds()
    {
        float x = Position.x;
        float y = Position.y;
        x = Mathf.Clamp(x, boundsWidth, bounds.x - boundsWidth);
        y = Mathf.Clamp(y, boundsWidth, bounds.y - boundsWidth);

        Position = new Vector2(x, y);
    }

    public void SetBounds(Vector2 bounds, float boundsWidth)
    {
        this.boundsWidth = boundsWidth;
        useBounds = true;
        this.bounds = bounds;
    }

    public void DisableBounds()
    {
        useBounds = false;
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
