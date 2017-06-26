using CCC.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Allegiance { Ally = 0, Neutral = 1, Enemy = 2, SmashBall = 3 }

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Unit : MonoBehaviour
{
    public const float deactivationRange = 10;

    [Header("Unit")]
    public Allegiance allegiance = Allegiance.Enemy;
    public bool overrideDeactivationRange = false;
    public float newDeactivationRange;

    protected float timeScale = 1;
    public Locker isAffectedByTimeScale = new Locker();

    public delegate void Unit_Event(Unit unit);
    public delegate void UnitMove_Event(Unit unit, Vector2 delta);
    public event Unit_Event onTimeScaleChange;
    public event UnitMove_Event onTeleportPosition;
    public event Unit_Event onDestroy;
    public event Unit_Event onDeath;

    public bool IsDead { get { return isDead; } }
    protected bool isDead = false;

    [Header("Border")]
    public bool canUseBorder = true;
    public float unitWidth;

    [System.NonSerialized]
    public Locker canMove = new Locker();
    [System.NonSerialized]
    public Locker canTurn = new Locker();

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
        protected set
        {
            if (canMove)
                rb.position = value;
        }
    }

    public float Rotation
    {
        get { return rb.rotation; }
        set
        {
            if (canTurn)
                rb.rotation = value;
        }
    }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<Transform>();
        canMove.onLockStateChange += OnLockMoveChange;
        canTurn.onLockStateChange += OnLockTurnChange;
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

    protected virtual void FixedUpdate()
    {
        if (Game.instance == null)
            return;

        if (canUseBorder && (Game.instance.unitSnap_horizontalBound || Game.instance.unitSnap_verticalBound))
            Position = RestrainToBounds(Position, Game.instance.unitSnap_horizontalBound, Game.instance.unitSnap_verticalBound);
    }

    public virtual void CheckActivation()
    {
        float delta = Mathf.Abs(Game.instance.gameCamera.Height - rb.position.y);
        float range = overrideDeactivationRange ? newDeactivationRange : deactivationRange;

        if (delta > range)
        {
            if (gameObject.activeSelf)
                gameObject.SetActive(false);
        }
        else
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);
        }
    }

    protected Vector2 RestrainToBounds(Vector2 vector, bool horizontal, bool vertical)
    {
        float x = vector.x;
        float y = vector.y;

        if (horizontal)
        {
            float rightBorder = Game.instance.gameCamera.ScreenSize.x / 2 - Game.instance.unitSnap_horizontalBorderWidth - (unitWidth / 2);
            x = Mathf.Clamp(x, -rightBorder, rightBorder);
        }
        if (vertical)
        {
            float halfHeight = Game.instance.gameCamera.ScreenSize.y / 2 - Game.instance.unitSnap_verticalBorderWidth - (unitWidth / 2);
            y = Mathf.Clamp(y, Game.instance.gameCamera.Height - halfHeight, Game.instance.gameCamera.Height + halfHeight);
        }

        return new Vector2(x, y);
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
        if (isDead)
            return;

        isDead = true;

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
                if(rb.bodyType != RigidbodyType2D.Static)
                rb.velocity *= value / timeScale;
            }

            timeScale = value;

            if (onTimeScaleChange != null)
                onTimeScaleChange.Invoke(this);
        }
    }
}
