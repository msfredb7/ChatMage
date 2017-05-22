using CCC.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class Unit : MonoBehaviour
{
    [System.Serializable]
    public class Unit_Event : UnityEvent<Unit> { }
    public float timeScale = 1;
    public Locker isAffectedByTimeScale = new Locker();

    public Unit_Event onTimeScaleChange = new Unit_Event();
    public Unit_Event onTeleportPosition = new Unit_Event();
    public Unit_Event onDestroy = new Unit_Event();

    protected Rigidbody2D rb;
    protected Transform tr;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<Transform>();
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
        onDestroy.Invoke(this);
    }

    public void TeleportPosition(Vector2 newPosition)
    {
        rb.position = newPosition;
        onTeleportPosition.Invoke(this);
    }

    public float TimeScale
    {
        get { return timeScale; }
        set
        {
            rb.velocity *= value / timeScale;
            timeScale = value;
            onTimeScaleChange.Invoke(this);
        }
    }
}
