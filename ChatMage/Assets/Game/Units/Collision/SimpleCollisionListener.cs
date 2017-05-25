using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimpleCollisionListener : MonoBehaviour
{
    public event Unit.Unit_Event onTriggerEnter;
    public event Unit.Unit_Event onTriggerExit;
    public event Unit.Unit_Event onCollisionEnter;
    public event Unit.Unit_Event onCollisionExit;


    public void OnTriggerExit2D(Collider2D other)
    {
        ColliderInfo info = other.GetComponent<ColliderInfo>();
        if (info == null)
            return;

        if (onTriggerExit != null)
            onTriggerExit.Invoke(info.parentUnit);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        ColliderInfo info = other.GetComponent<ColliderInfo>();
        if (info == null)
            return;

        if (onTriggerEnter != null)
            onTriggerEnter.Invoke(info.parentUnit);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        ColliderInfo info = collision.collider.GetComponent<ColliderInfo>();
        if (info == null)
            return;

        if (onCollisionEnter != null)
            onCollisionEnter(info.parentUnit);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        ColliderInfo info = collision.collider.GetComponent<ColliderInfo>();
        if (info == null)
            return;

        if (onCollisionEnter != null)
            onCollisionEnter(info.parentUnit);
    }
}
