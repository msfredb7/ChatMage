using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Utility;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class MovingUnit : Unit
{
    [System.NonSerialized]
    public Rigidbody2D rb;

    [System.NonSerialized]
    public Locker canMove = new Locker();
    [System.NonSerialized]
    public Locker canTurn = new Locker();


    protected Vector2 sleepRbVelocity = Vector2.zero;
    protected float sleepRbAngVelocity = 0;

    protected override void Awake()
    {
        base.Awake();

        rb = GetComponent<Rigidbody2D>();
        canMove.onLockStateChange += OnLockMoveChange;
        canTurn.onLockStateChange += OnLockTurnChange;
    }

    public override Vector3 WorldDirection()
    {
        return Speed.normalized;
    }
    public override Vector2 WorldDirection2D()
    {
        return Speed.normalized;
    }

    public Vector2 Speed
    {
        get { return rb.velocity; }
        set
        {
            if (canMove)
                rb.velocity = value;
        }
    }

    public override Vector2 Position
    {
        get { return rb.position; }
        protected set
        {
            if (canMove)
                rb.position = value;
        }
    }

    public override float Rotation
    {
        get { return rb.rotation; }
        set
        {
            if (canTurn)
                rb.rotation = value;
        }
    }

    void OnLockTurnChange(bool state)
    {
        rb.freezeRotation = !state;
    }
    void OnLockMoveChange(bool state)
    {
        if (rb.freezeRotation)
            rb.constraints = state ? RigidbodyConstraints2D.FreezeRotation : RigidbodyConstraints2D.FreezeAll;
        else
            rb.constraints = state ? RigidbodyConstraints2D.None : RigidbodyConstraints2D.FreezePosition;
    }

    public virtual void SaveRigidbody()
    {
        if (rb.bodyType == RigidbodyType2D.Static)
            return;

        sleepRbVelocity = rb.velocity;
        sleepRbAngVelocity = rb.angularVelocity;
    }

    public virtual void LoadRigidbody()
    {
        if (rb.bodyType == RigidbodyType2D.Static)
            return;

        rb.velocity = sleepRbVelocity;
        rb.angularVelocity = sleepRbAngVelocity;
    }

    public override float TimeScale
    {
        get
        {
            return base.TimeScale;
        }

        set
        {
            float oldTimescale = timeScale;

            base.TimeScale = value;

            var mult = value / oldTimescale;
            if (rb.bodyType != RigidbodyType2D.Static)
                rb.velocity *= mult;
            rb.angularDrag *= mult;
            rb.drag *= mult;
        }
    }
}
