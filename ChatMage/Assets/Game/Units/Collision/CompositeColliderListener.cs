using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CompositeColliderListener : ColliderListener
{
    public event TriggerEvent onTriggerEnter;
    public event TriggerEvent onTriggerExit;
    public event CollisionEvent onCollisionEnter;
    public event CollisionEvent onCollisionExit;

    [Header("Settings")]
    public bool useTrigger;
    public bool useCollision;
    public bool groupByUnits = false;

    public override TriggerEvent OnTriggerEnter { get { return onTriggerEnter; } set { onTriggerEnter = value; } }
    public override TriggerEvent OnTriggerExit { get { return onTriggerExit; } set { onTriggerExit = value; } }
    public override CollisionEvent OnCollisionEnter { get { return onCollisionEnter; } set { onCollisionEnter = value; } }
    public override CollisionEvent OnCollisionExit { get { return onCollisionExit; } set { onCollisionExit = value; } }

    private SingleColliderFilter filter;
    private bool enterTriggered = false;
    private bool exitTriggered = false;

    void Update()
    {
        if (filter != null)
            filter.RemoteUpdate();
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (!useTrigger)
            return;

        CheckTriggerResources();

        ColliderInfo info = collider.GetComponent<ColliderInfo>();

        if (info != null)
            filter.OnTriggerEnter(info);

        if (enterTriggered)
        {
            enterTriggered = false;
            if (onTriggerEnter != null)
                onTriggerEnter(info, this);
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (!useTrigger)
            return;

        CheckTriggerResources();

        ColliderInfo info = collider.GetComponent<ColliderInfo>();

        if (info != null)
            filter.OnTriggerExit(info);

        if (exitTriggered)
        {
            exitTriggered = false;
            if (onTriggerExit != null)
                onTriggerExit(info, this);
        }
    }

    void EnterTriggered(Unit unit)
    {
        enterTriggered = true;
    }
    void ExitTriggered(Unit unit)
    {
        exitTriggered = true;
    }

    void CheckTriggerResources()
    {
        if (filter == null)
        {
            filter = new SingleColliderFilter(groupByUnits);
            filter.onUnitAdded = EnterTriggered;
            filter.onUnitRemoved = ExitTriggered;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!useCollision)
            return;

        ColliderInfo info = collision.collider.GetComponent<ColliderInfo>();
        if (info == null)
            return;

        if (onCollisionEnter != null)
            onCollisionEnter(info, collision, this);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (!useCollision)
            return;

        ColliderInfo info = collision.collider.GetComponent<ColliderInfo>();
        if (info == null)
            return;

        if (onCollisionExit != null)
            onCollisionEnter(info, collision, this);
    }
}
