using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CCC.Utility;

public class Vehicle : MovingUnit
{
    public float moveSpeed = 1;
    public float targetDirection;
    public bool rotationSetsTargetDirection = false;
    public bool useWeight;
    [Range(0, 1)]
    public float weight = 0.1f;
    /// <summary>
    /// Si locked, le véhicule n'accélaire pas.
    /// </summary>
    public Locker canAccelerate = new Locker();

    private float bumpTime = 0;

    //Events
    public class VehicleEvent : UnityEvent<Vehicle> { }
    [System.NonSerialized]
    public VehicleEvent onBump = new VehicleEvent();
    [System.NonSerialized]
    public VehicleEvent onBumpComplete = new VehicleEvent();
    [System.NonSerialized]
    public VehicleEvent onTeleportDirection = new VehicleEvent();

    protected override void FixedUpdate()
    {
        if (FixedDeltaTime() <= 0)
            return;

        if (rotationSetsTargetDirection)
        {
            Vector3 forward = tr.right;
            targetDirection =  VectorToAngle(forward);
        }

        UpdateBumpTime();

        if (canAccelerate)
            GroundedUpdate();
        
        base.FixedUpdate();
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
        canAccelerate.Unlock("bump");
        onBumpComplete.Invoke(this);
    }

    void GroundedUpdate()
    {
        if (timeScale <= 0)
            return;

        Vector2 vDir = WorldDirection2D() * moveSpeed * timeScale;
        if (useWeight)
            Speed = Vector2.Lerp(
                Speed,  //Current
                vDir,   //target
                FixedLerp.Fix(
                    weight >= 1f ? 1 : weight / 10,
                    FPSCounter.GetFixedFPS() / timeScale));
        else
            Speed = vDir;
    }
    
    public void Bump(Vector2 velocity, float duration, BumpMode bumpMode)
    {
        switch (bumpMode)
        {
            default:
            case BumpMode.VelocityAdd:
                Speed += velocity;
                break;
            case BumpMode.VelocityChange:
                Speed = velocity;
                break;
        }
        if (duration > bumpTime)
        {
            bumpTime = duration;
            canAccelerate.LockUnique("bump");
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
    public static Vector2 AngleToVector(float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
    }

    public void TeleportDirection(float newDirection)
    {
        targetDirection = newDirection;
        rb.rotation = newDirection;
        onTeleportDirection.Invoke(this);
    }
}

public enum BumpMode { VelocityAdd, VelocityChange }