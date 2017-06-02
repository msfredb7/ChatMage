using CCC.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Allegiance { Ally = 0, Neutral = 1, Enemy = 2, SmashBall = 3 }

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Unit : MonoBehaviour
{
    [Header("Unit")]
    public Allegiance allegiance = Allegiance.Enemy;
    protected float timeScale = 1;
    public Locker isAffectedByTimeScale = new Locker();

    public delegate void Unit_Event(Unit unit);
    public delegate void UnitMove_Event(Unit unit, Vector2 delta);
    public event Unit_Event onTimeScaleChange;
    public event UnitMove_Event onTeleportPosition;
    public event Unit_Event onDestroy;
    public event Unit_Event onDeath;

    [Header("Border")]
    public bool canUseBorder = true;
    public float unitWidth;
    [Header("Border(will be changed on spawn)")]
    public bool horizontalBound;
    public float horizontalBorderWidth;
    public bool verticalBound;
    public float verticalBorderWidth;

    [System.NonSerialized]
    public Rigidbody2D rb;
    protected Transform tr;

    protected Vector2 sleepRbVelocity = Vector2.zero;
    protected float sleepRbAngVelocity = 0;
    
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

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<Transform>();
    }

    protected virtual void FixedUpdate()
    {
        if (canUseBorder || horizontalBound || verticalBound)
            RestrainToBounds();
    }

    void RestrainToBounds()
    {
        float x = Position.x;
        float y = Position.y;

        if (horizontalBound)
        {
            float rightBorder = Game.instance.gameCamera.ScreenSize.x / 2 - horizontalBorderWidth - (unitWidth / 2);
            x = Mathf.Clamp(x, -rightBorder, rightBorder);
        }
        if (verticalBound)
        {
            float halfHeight = Game.instance.gameCamera.ScreenSize.y / 2 - verticalBorderWidth - (unitWidth / 2);
            y = Mathf.Clamp(y, Game.instance.gameCamera.Height - halfHeight, Game.instance.gameCamera.Height + halfHeight);
        }

        Position = new Vector2(x, y);
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

            //On stoppe le temps ? Si oui, prendre en note la velocit� original
            if (value == 0)
            {
                referenceVelocity = rb.velocity / timeScale;

                rb.velocity = Vector2.zero;
                wasStopped = true;
            }
            else if (wasStopped) // Sinon, est-ce qu'on �tait arret� auparavant ? Si oui, utilis� la formule
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
