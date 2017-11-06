using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimpleColliderListener : ColliderListener
{
    [Header("Settings")]
    public bool useTrigger;
    public bool useCollision;

    public event TriggerEvent onTriggerEnter;
    public event TriggerEvent onTriggerExit;
    public event CollisionEvent onCollisionEnter;
    public event CollisionEvent onCollisionExit;
    
    public override TriggerEvent OnTriggerEnter { get { return onTriggerEnter; } set { onTriggerEnter = value; } }
    public override TriggerEvent OnTriggerExit { get { return onTriggerExit; } set { onTriggerExit = value; } }
    public override CollisionEvent OnCollisionEnter { get { return onCollisionEnter; } set { onCollisionEnter = value; } }
    public override CollisionEvent OnCollisionExit { get { return onCollisionExit; } set { onCollisionExit = value; } }

    public virtual void OnTriggerExit2D(Collider2D other)
    {
        if (!useTrigger)
            return;

        ColliderInfo info = other.GetComponent<ColliderInfo>();
        if (info == null)
            return;

        if (onTriggerExit != null)
            onTriggerExit.Invoke(info, this);
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (!useTrigger)
            return;

        ColliderInfo info = other.GetComponent<ColliderInfo>();
        if (info == null)
            return;

        if (onTriggerEnter != null)
            onTriggerEnter.Invoke(info, this);
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        //print("collision between: " + collision.collider.gameObject.name + " / " + collision.otherCollider.gameObject.name);
        if (!useCollision)
            return;

        ColliderInfo info = collision.collider.GetComponent<ColliderInfo>();
        if (info == null)
            return;

        if (onCollisionEnter != null)
            onCollisionEnter(info, collision, this);
    }

    public virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (!useCollision)
            return;

        ColliderInfo info = collision.collider.GetComponent<ColliderInfo>();
        if (info == null)
            return;

        if (onCollisionExit != null)
            onCollisionExit(info, collision, this);
    }
}
