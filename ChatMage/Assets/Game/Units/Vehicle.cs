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
    private float bumpTime = 0;

    //Events
    public class VehicleEvent : UnityEvent<Vehicle> { }
    [System.NonSerialized]
    public VehicleEvent onBump = new VehicleEvent();
    [System.NonSerialized]
    public VehicleEvent onBumpComplete = new VehicleEvent();
    [System.NonSerialized]
    public VehicleEvent onTeleportDirection = new VehicleEvent();

    protected override void Update()
    {
        if (DeltaTime() <= 0)
            return;

        UpdateBumpTime();

        if (isGrounded)
            GroundedUpdate();

        Vector3 wasPos = tr.position;

        //update position
        base.Update();

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
    public static float VectorToAngle(Vector2 dir)
    {
        if (dir.x < 0)
            return Mathf.Atan(dir.y / dir.x)* Mathf.Rad2Deg + 180;
        return Mathf.Atan(dir.y / dir.x) * Mathf.Rad2Deg;
    }

    public void TeleportDirection(float newDirection)
    {
        targetDirection = newDirection;
        onTeleportDirection.Invoke(this);
    }
}

public enum BumpMode { VelocityAdd, VelocityChange }