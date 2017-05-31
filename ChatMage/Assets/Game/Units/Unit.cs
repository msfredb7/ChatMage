using CCC.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Allegiance { Ally = 0, Neutral = 1, Enemy = 2 }

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Unit : MonoBehaviour
{
    public Allegiance allegiance = Allegiance.Enemy;
    protected float timeScale = 1;
    public Locker isAffectedByTimeScale = new Locker();

    public delegate void Unit_Event(Unit unit);
    public delegate void UnitMove_Event(Unit unit, Vector2 delta);
    public event Unit_Event onTimeScaleChange;
    public event UnitMove_Event onTeleportPosition;
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
    //public MovingPlatform movingPlatform;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<Transform>();
    }

    protected virtual void FixedUpdate()
    {
        if (useMovingPlatform && Game.instance.map.rubanPlayer != null)
            tr.position += Vector3.up * Game.instance.map.rubanPlayer.GetVerticalSpeed() * Time.fixedDeltaTime;
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
        Vector2 delta = newPosition - rb.position;
        rb.position = newPosition;
        if (onTeleportPosition != null)
            onTeleportPosition.Invoke(this, delta);
    }

    protected virtual void Die()
    {
        if (onDeath != null)
            onDeath(this);
    }

    [System.NonSerialized]
    private Vector2 referenceVelocity;
    [System.NonSerialized]
    private bool wasStopped = false;

    public float TimeScale
    {
        get { return timeScale; }
        set
        {
            if (value == timeScale)
                return;

            if (value < 0)
                value = 0;

            //On stoppe le temps ? Si oui, prendre en note la velocité original
            if (value == 0)
            {
                referenceVelocity = rb.velocity / timeScale;

                rb.velocity = Vector2.zero;
                wasStopped = true;
            }
            else if (wasStopped) // Sinon, est-ce qu'on était arreté auparavant ? Si oui, utilisé la formule
            {
                rb.velocity = referenceVelocity * value;
                wasStopped = false;
            }
            else    // Sinon, formule standard
            {
                rb.velocity *= value / timeScale;
            }

            timeScale = value;

            if (onTimeScaleChange != null)
                onTimeScaleChange.Invoke(this);
        }
    }
}
