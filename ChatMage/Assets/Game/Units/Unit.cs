using CCC.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Allegiance { Ally = 0, Neutral = 1, Enemy = 2, SmashBall = 3 }

public abstract class Unit : MonoBehaviour
{
    private const float MIN_TIMESCALE = .02f;

    [Header("Unit")]
    public Allegiance allegiance = Allegiance.Enemy;

    protected float timeScale = 1;

    public delegate void Unit_Event(Unit unit);
    public delegate void UnitMove_Event(Unit unit, Vector2 delta);
    public event Unit_Event onTimeScaleChange;
    public event UnitMove_Event onTeleportPosition;
    public event Unit_Event onDestroy;
    public event Unit_Event onDeath;

    [System.NonSerialized]
    public List<string> marks = new List<string>();

    public bool IsDead { get { return isDead; } }
    protected bool isDead = false;
    protected bool isDestroying = false;

    protected Transform tr;


    //Ici, on a une referance vers les nodes de linkedlist de Game. En gros, lorsque la unit est delete, et
    // qu'elle doit etre elever de la liste, game n'a pas a chercher dans la liste pour la bonne node car
    // elle est enregistrer ici. O(n) -> O(1)
    [System.NonSerialized]
    public LinkedListNode<Unit> stdNode;
    public LinkedListNode<Unit> attackableNode;

    [System.NonSerialized]
    protected List<BaseBuff> buffs;

    public virtual Vector2 Position
    {
        get { return tr.position; }
        protected set
        {
            tr.position = value;
        }
    }

    public virtual float Rotation
    {
        get { return tr.rotation.eulerAngles.z; }
        set
        {
            tr.rotation = Quaternion.Euler(Vector3.forward * value);
        }
    }

    protected virtual void Awake()
    {
        tr = GetComponent<Transform>();
    }

    protected virtual void FixedUpdate()
    {
        if (Game.instance == null)
            return;
    }

    protected virtual void Update()
    {
        if (buffs != null && buffs.Count > 0)
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
        return Time.deltaTime * timeScale;
    }
    public float FixedDeltaTime()
    {
        return Time.fixedDeltaTime * timeScale;
    }

    protected virtual void OnDestroy()
    {
        if (onDestroy != null)
            onDestroy(this);
    }

    public void TeleportPosition(Vector2 newPosition)
    {
        Vector2 delta = newPosition - Position;
        Position = newPosition;
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

        if (Game.instance != null)
            Game.instance.StartCoroutine(LateDestroy());
    }

    private IEnumerator LateDestroy()
    {
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        Destroy(gameObject);
    }

    public virtual float TimeScale
    {
        get { return timeScale; }
        set
        {
            if (value == timeScale)
                return;

            if (value < MIN_TIMESCALE)
                value = MIN_TIMESCALE;

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

    public int CheckBuffs_Attacked(ColliderInfo on, int amount, Unit otherUnit, ColliderInfo source = null)
    {
        if (buffs != null)
            for (int i = 0; i < buffs.Count; i++)
            {
                if (buffs[i] is IAttackable)
                    amount = (buffs[i] as IAttackable).Attacked(on, amount, otherUnit, source);
            }

        return amount;
    }

    public bool HasBuffOfType(System.Type type)
    {
        if(buffs != null)
        {
            for (int i = 0; i < buffs.Count; i++)
            {
                if (buffs[i].GetType() == type)
                    return true;
            }
        }
        return false;
    }

    public static bool HasPresence(Unit unit)
    {
        return unit != null && !unit.IsDead && unit.gameObject.activeSelf && (!(unit is IVisible) || (unit as IVisible).IsVisible());
    }
}
