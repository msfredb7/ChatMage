using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionListener : MonoBehaviour
{
    [System.Serializable]
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

    public bool isIntersecting = false;



    public void OnTriggerEnter2D(Collider2D other)
    {
        //Info du collider
        ColliderInfo info = other.GetComponent<ColliderInfo>();

        if (info == null)
            return;

        GameObject group = other.gameObject;
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

    public void OnTriggerExit2D(Collider2D other)
    {
        //Info du collider
        ColliderInfo info = other.GetComponent<ColliderInfo>();

        if (info == null)
            return;

        GameObject group = other.gameObject;
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

    public void OnEnter(Unit unit)
    {
        isIntersecting = true;

        onEnter.Invoke(unit);
    }

    public void OnExit(Unit unit)
    {
        if (inContactWith.Count > 0)
            isIntersecting = true;
        else
            isIntersecting = false;

        onExit.Invoke(unit);
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
