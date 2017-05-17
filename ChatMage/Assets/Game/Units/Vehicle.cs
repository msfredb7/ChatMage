using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    protected override void Update()
    {
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
                    FPSCounter.GetFPS()/timeScale));
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
