﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionListener : MonoBehaviour
{

    public class ObjectCollider
    {
        public GameObject group;
        public int collisionCount;
        public Unit unit;

        public ObjectCollider(GameObject group, Unit unit, int collisionCount = 0)
        {
            this.unit = unit;
            this.group = group;
            this.collisionCount = collisionCount;
        }
    }

    //public UnityEvent onStay = new UnityEvent();
    public Unit.Unit_Event onEnter = new Unit.Unit_Event();
    public Unit.Unit_Event onExit = new Unit.Unit_Event();

    public List<ObjectCollider> inContactWith = new List<ObjectCollider>();

    public bool onInside = false;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Info du collider
        ColliderInfo info = collision.GetComponent<ColliderInfo>();

        if (info == null)
            return;

        GameObject group = collision.gameObject;
        if (info.groupParent != null)
            group = info.groupParent;

        //Objet parent
        ObjectCollider obj = GetObjectByGroup(group);

        //Déjà là ?
        if (obj != null)
            obj.collisionCount++;
        else
        {

            inContactWith.Add(new ObjectCollider(group, info.parentUnit, 1));
            OnEnter(info.parentUnit);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        //Info du collider
        ColliderInfo info = collision.GetComponent<ColliderInfo>();

        if (info == null)
            return;

        GameObject group = collision.gameObject;
        if (info.groupParent != null)
            group = info.groupParent;

        ObjectCollider obj = GetObjectByGroup(group);
        obj.collisionCount--;
        if (obj.collisionCount <= 0)
        {
            inContactWith.Remove(obj);
            OnExit(obj.unit);
        }
    }

    /*
    public void OnTriggerStay2D(Collider2D collision)
    {
        OnStay();
    }

    public void OnStay()
    {
        onStay.Invoke();
    }
    */

    public void OnEnter(Unit unit)
    {
        onEnter.Invoke(unit);
        onInside = true;
    }

    public void OnExit(Unit unit)
    {
        onExit.Invoke(unit);

        if (inContactWith.Count > 0)
            onInside = true;
        else
            onInside = false;
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
