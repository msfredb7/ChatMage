using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Utility;

public abstract class Vehicle : MovingUnit
{
    public float moveSpeed = 1;
    public float direction;
    [Range(0, 1)]
    public float weight = 0.1f;
    /// <summary>
    /// Si locked, le véhicule n'accélaire pas.
    /// </summary>
    public Locker isGrounded = new Locker();


    public Vector3 CurrentVelocity
    {
        get { return vSpeed; }
    }

    private Transform tr;
    private Vector3 vSpeed;
    private Vector2 bounds = new Vector2(10, 10);
    private bool useBounds = false;

    void Start()
    {
        tr = GetComponent<Transform>();
    }

    void Update()
    {
        Vector3 vDir = WorldDirection() * moveSpeed;
        if (isGrounded)
        {
            if (UseWeight())
                vSpeed = Vector3.Lerp(vSpeed, vDir, FixedLerp.Fix(weight >= 1f ? 1 : weight / 10));
            else
                vSpeed = vDir;
        }

        Vector3 wasPos = tr.position;
        tr.position += vSpeed * Time.deltaTime;

        if (useBounds)
            tr.position = new Vector3(
                Mathf.Max(0, Mathf.Min(bounds.x, tr.position.x)),       //x
                Mathf.Max(0, Mathf.Min(bounds.y, tr.position.y)),       //y
                tr.position.z);                                         //z

        vSpeed = (tr.position - wasPos) / Time.deltaTime;
    }

    public void SetWorldBounds(Vector2 bounds)
    {
        this.bounds = bounds;
        useBounds = true;
    }

    public Vector3 WorldDirection()
    {
        float rad = direction * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0);
    }
    public Vector2 WorldDirection2D()
    {
        float rad = direction * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
    }

    protected abstract bool UseWeight();
}
