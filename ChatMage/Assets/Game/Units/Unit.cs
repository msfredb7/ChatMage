using CCC.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class Unit : MonoBehaviour
{
    public float timeScale = 1;
    public Locker isAffectedByTimeScale = new Locker();

    public delegate void Unit_Event(Unit unit);
    public event Unit_Event onTimeScaleChange;
    public event Unit_Event onTeleportPosition;
    public event Unit_Event onDestroy;
    public event Unit_Event onDeath;

    [System.NonSerialized]
    public Rigidbody2D rb;
    protected Transform tr;

    public Vector2 Speed
    {
        get { return rb.velocity; }
        set { rb.velocity = value; }
    }

    public Vector2 Position
    {
        get { return rb.position; }
        protected set { rb.position = value; }
    }

    public float Rotation { get { return rb.rotation; } set { rb.rotation = value; } }

    public bool useMovingPlatform = true;
    public MovingPlatform movingPlatform;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<Transform>();
    }

    protected virtual void FixedUpdate()
    {
        if (movingPlatform != null && useMovingPlatform)
            tr.position += Vector3.up * movingPlatform.GetVerticalSpeed() * Time.fixedDeltaTime;
    }

    public virtual Vector3 WorldDirection()
    {
        return Vector3.up;
    }
    public virtual Vector2 WorldDirection2D()
    {
        return Vector2.up;
    }
    public float DeltaTime()
    {
        return isAffectedByTimeScale ? Time.deltaTime * timeScale : Time.deltaTime;
    }
    public float FixedDeltaTime()
    {
        return isAffectedByTimeScale ? Time.fixedDeltaTime * timeScale : Time.fixedDeltaTime;
    }

    void OnDestroy()
    {
        if (onDestroy != null)
            onDestroy(this);
    }

    public void TeleportPosition(Vector2 newPosition)
    {
        rb.position = newPosition;
        if (onTeleportPosition != null)
            onTeleportPosition.Invoke(this);
    }

    protected virtual void Die()
    {
        if(onDeath != null)
            onDeath(this);
    }

    public float TimeScale
    {
        get { return timeScale; }
        set
        {
            rb.velocity *= value / timeScale;
            timeScale = value;
            if (onTimeScaleChange != null)
                onTimeScaleChange.Invoke(this);
        }
    }
}
