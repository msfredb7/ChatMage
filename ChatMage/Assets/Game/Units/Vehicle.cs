using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CCC.Utility;

public class Vehicle : MovingUnit
{
    public float moveSpeed = 1;
    public float targetDirection;
    public bool useWeight;
    [Range(0, 1)]
    public float weight = 0.1f;
    /// <summary>
    /// Si locked, le véhicule n'accélaire pas.
    /// </summary>
    public Locker isGrounded = new Locker();

    public Vector3 CurrentVelocity
    {
        get { return speed; }
    }
    private Vector2 bounds = new Vector2(10, 10);
    private bool useBounds = false;
    private float bumpTime = 0;

    //Events
    public class VehicleEvent : UnityEvent<Vehicle> { }
    [System.NonSerialized]
    public VehicleEvent onBump = new VehicleEvent();
    [System.NonSerialized]
    public VehicleEvent onBumpComplete = new VehicleEvent();

    protected override void Update()
    {
        UpdateBumpTime();

        if (isGrounded)
            GroundedUpdate();

        Vector3 wasPos = tr.position;

        //update position
        base.Update();

        if (useBounds)
            RestrainToBounds();

        //record actual speed
        speed = (tr.position - wasPos) / DeltaTime();
    }

    void UpdateBumpTime()
    {
        bool wasBumped = bumpTime > 0;
        if (bumpTime > 0)
            bumpTime -= DeltaTime();

        if (bumpTime <= 0 && wasBumped)
            OnCompleteBump();
    }

    void OnCompleteBump()
    {
        isGrounded.Unlock("bump");
        onBumpComplete.Invoke(this);
    }

    void GroundedUpdate()
    {
        if (timeScale <= 0)
            return;

        Vector3 vDir = WorldDirection() * moveSpeed;
        if (useWeight)
            speed = Vector3.Lerp(
                speed,  //Current
                vDir,   //target
                FixedLerp.Fix(
                    weight >= 1f ? 1 : weight / 10,
                    FPSCounter.GetFPS() / timeScale));
        else
            speed = vDir;
    }

    void RestrainToBounds()
    {
        tr.position = new Vector3(
            Mathf.Max(0, Mathf.Min(bounds.x, tr.position.x)),       //x
            Mathf.Max(0, Mathf.Min(bounds.y, tr.position.y)),       //y
            tr.position.z);                                         //z
    }

    public void Bump(Vector2 velocity, float duration, BumpMode bumpMode)
    {
        Vector3 vel3 = new Vector3(velocity.x, velocity.y, 0);
        switch (bumpMode)
        {
            default:
            case BumpMode.VelocityAdd:
                speed += velocity;
                break;
            case BumpMode.VelocityChange:
                speed = velocity;
                break;
        }
        if (duration > bumpTime)
        {
            bumpTime = duration;
            isGrounded.LockUnique("bump");
        }

        onBump.Invoke(this);
    }

    public void SetWorldBounds(Vector2 bounds)
    {
        this.bounds = bounds;
        useBounds = true;
    }

    public override Vector3 WorldDirection()
    {
        float rad = targetDirection * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0);
    }
    public override Vector2 WorldDirection2D()
    {
        float rad = targetDirection * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
    }
}

public enum BumpMode { VelocityAdd, VelocityChange }