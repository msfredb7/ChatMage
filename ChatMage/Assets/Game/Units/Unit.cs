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
    public bool checkDeactivation = false;

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
    protected bool isDestroying = false;

    [System.NonSerialized]
    public Locker canMove = new Locker();
    [System.NonSerialized]
    public Locker canTurn = new Locker();

    [System.NonSerialized]
    public Rigidbody2D rb;
    protected Transform tr;

    protected Vector2 sleepRbVelocity = Vector2.zero;
    protected float sleepRbAngVelocity = 0;

    [System.NonSerialized]
    private List<BaseBuff> buffs;

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
    }

    protected virtual void Update()
    {
        if(buffs != null && buffs.Count > 0)
        {
            float worldDeltaTime = Game.instance.worldTimeScale * Time.deltaTime;
            float localDeltaTime = DeltaTime();
            for (int i = 0; i < buffs.Count; i++)
            {
                if (buffs[i].DecreaseDuration(worldDeltaTime, localDeltaTime) <= 0)
                {
                    RemoveBuffAt(i);
                    i--;
                }
            }
        }
    }

    public virtual void CheckActivation()
    {
        if (!checkDeactivation)
            return;

        float delta = Mathf.Abs(Game.instance.gameCamera.Height - rb.position.y);
        float range = deactivationRange;

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

    protected virtual void OnDestroy()
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

    public void ForceDie()
    {
        Die();
    }

    protected void Destroy()
    {
        if (isDestroying)
            return;
        isDestroying = true;
        gameObject.SetActive(false);
        checkDeactivation = false;
        if (Game.instance != null)
            Game.instance.StartCoroutine(LateDestroy());
    }

    private IEnumerator LateDestroy()
    {
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        Destroy(gameObject);
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
                value = 0.02f;

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
                if (rb.bodyType != RigidbodyType2D.Static)
                    rb.velocity *= value / timeScale;
            }

            timeScale = value;

            if (onTimeScaleChange != null)
                onTimeScaleChange.Invoke(this);
        }
    }

    public void AddBuff(BaseBuff buff)
    {
        if (buffs == null)
            buffs = new List<BaseBuff>();

        buffs.Add(buff);
        buff.ApplyEffect(this);
    }

    private void RemoveBuffAt(int i)
    {
        buffs[i].RemoveEffect(this);
        buffs.RemoveAt(i);
    }
}
