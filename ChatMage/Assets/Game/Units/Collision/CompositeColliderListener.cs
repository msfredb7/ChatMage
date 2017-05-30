using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CompositeColliderListener : ColliderListener
{
    [System.Serializable]
    public class ObjectCollider
    {
        public GameObject group;
        public int collisionCount;
        public List<Collider2D> includedColliders = new List<Collider2D>();
        public Unit unit;

        public ObjectCollider(GameObject group, Unit unit)
        {
            this.unit = unit;
            this.group = group;
        }
    }

    public event TriggerEvent onTriggerEnter;
    public event TriggerEvent onTriggerExit;
    public event CollisionEvent onCollisionEnter;
    public event CollisionEvent onCollisionExit;
    [Header("Settings")]
    public bool useTrigger;
    public bool useCollision;

    [Header("Dynamic List")]
    public List<ObjectCollider> inContactWith = new List<ObjectCollider>();
    private List<Collider2D> forgetList = new List<Collider2D>(3);

    public override TriggerEvent OnTriggerEnter { get { return onTriggerEnter; } set { onTriggerEnter = value; } }
    public override TriggerEvent OnTriggerExit { get { return onTriggerExit; } set { onTriggerExit = value; } }
    public override CollisionEvent OnCollisionEnter { get { return onCollisionEnter; } set { onCollisionEnter = value; } }
    public override CollisionEvent OnCollisionExit { get { return onCollisionExit; } set { onCollisionExit = value; } }

    void Update()
    {
        forgetList.Clear();
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (!useTrigger)
            return;

        //Info du collider
        ColliderInfo info = other.GetComponent<ColliderInfo>();

        if (info == null)
            return;

        GameObject group = info.GroupParent;
        ObjectCollider obj = GetObjectByGroup(group);


        if (obj == null || !obj.includedColliders.Contains(other))
        {
            forgetList.Add(other);  //L'objet est entré et sortie dans la même frame
        }
        else
        {
            obj.includedColliders.Remove(other);
            obj.collisionCount--;
            if (obj.collisionCount <= 0)
            {
                inContactWith.Remove(obj);
                OnExit(info);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!useTrigger)
            return;

        //Est-ce que l'objet est entré/sortie dans la même frame ?
        int i = forgetList.IndexOf(other);
        if (i >= 0)
        {
            forgetList.RemoveAt(i);
            return;
        }


        //Info du collider
        ColliderInfo info = other.GetComponent<ColliderInfo>();

        if (info == null)
            return;

        GameObject group = info.GroupParent;
        ObjectCollider obj = GetObjectByGroup(group);

        if (obj == null)
            obj = new ObjectCollider(group, info.parentUnit);

        obj.includedColliders.Add(other);
        obj.collisionCount++;
        if (obj.collisionCount == 1)
        {
            inContactWith.Add(obj);
            OnEnter(info);
        }
    }

    public void OnEnter(ColliderInfo otherInfo)
    {
        if (onTriggerEnter != null)
            onTriggerEnter(otherInfo, this);
    }

    public void OnExit(ColliderInfo otherInfo)
    {
        if (onTriggerExit != null)
            onTriggerExit(otherInfo, this);
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

    public ObjectCollider GetObjectByGroup(GameObject group)
    {
        for (int i = 0; i < inContactWith.Count; i++)
        {
            if (inContactWith[i].group == group)
                return inContactWith[i];
        }
        return null;
    }
}
