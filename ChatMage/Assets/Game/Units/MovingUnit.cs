using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Utility;

public class MovingUnit : Unit
{
    [System.NonSerialized]
    public Vector2 speed;
    [System.NonSerialized]
    public Locker canMove = new Locker();
    [System.NonSerialized]
    public bool useBounds = false;

    protected Vector2 bounds = new Vector2(10, 10);
    protected Transform tr;

    protected virtual void Start()
    {
        tr = GetComponent<Transform>();
    }

    protected virtual void Update()
    {
        Vector2 v = speed * DeltaTime();
        Move(v);

        if (useBounds)
            RestrainToBounds();
    }

    public void Move(Vector2 delta)
    {
        tr.position += new Vector3(delta.x, delta.y, 0);
    }

    void RestrainToBounds()
    {
        tr.position = new Vector3(
            Mathf.Max(0, Mathf.Min(bounds.x, tr.position.x)),       //x
            Mathf.Max(0, Mathf.Min(bounds.y, tr.position.y)),       //y
            tr.position.z);                                         //z
    }

    public void SetWorldBounds(Vector2 bounds)
    {
        this.bounds = bounds;
        useBounds = true;
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
