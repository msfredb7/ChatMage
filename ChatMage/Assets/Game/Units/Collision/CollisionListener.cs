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
        public List<Collider2D> includedColliders = new List<Collider2D>();
        public Unit unit;

        public ObjectCollider(GameObject group, Unit unit)
        {
            this.unit = unit;
            this.group = group;
        }
    }

    //public UnityEvent onStay = new UnityEvent();
    public event Unit.Unit_Event onEnter;
    public event Unit.Unit_Event onExit;

    public List<ObjectCollider> inContactWith = new List<ObjectCollider>();
    public List<Collider2D> forgetList = new List<Collider2D>(3);

    public bool isIntersecting = false;


    void Update()
    {
        forgetList.Clear();
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
                OnExit(obj.unit);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
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

        GameObject group = other.gameObject;
        if (info.groupParent != null)
            group = info.groupParent;

        //Objet parent
        ObjectCollider obj = GetObjectByGroup(group);

        if (obj == null)
            obj = new ObjectCollider(group, info.parentUnit);

        obj.includedColliders.Add(other);
        obj.collisionCount++;
        if (obj.collisionCount == 1)
        {
            inContactWith.Add(obj);
            OnEnter(info.parentUnit);
        }
    }

    public void OnEnter(Unit unit)
    {
        isIntersecting = true;

        if (onEnter != null)
            onEnter.Invoke(unit);
    }

    public void OnExit(Unit unit)
    {
        if (inContactWith.Count > 0)
            isIntersecting = true;
        else
            isIntersecting = false;

        if (onExit != null)
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
