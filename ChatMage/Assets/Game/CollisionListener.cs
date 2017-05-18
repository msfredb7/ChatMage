using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionListener : MonoBehaviour {

    public class ObjectCollider
    {
        public Collider2D collider;
        public int collisionCount;

        public ObjectCollider(Collider2D collider, int collisionCount = 0)
        {
            this.collider = collider;
            this.collisionCount = collisionCount;
        }
    }

    //public UnityEvent onStay = new UnityEvent();
    public UnityEvent onEnter = new UnityEvent();
    public UnityEvent onExit = new UnityEvent();

    public List<ObjectCollider> inContactWith = new List<ObjectCollider>();

    public bool onInside = false;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (HasAlreadyCollide(collision))
            GetObjectByCollider(collision).collisionCount++;
        else
        {
            inContactWith.Add(new ObjectCollider(collision, 1));
            OnEnter();
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        ObjectCollider collider = GetObjectByCollider(collision);
        collider.collisionCount--;
        if (collider.collisionCount <= 0)
        {
            inContactWith.Remove(collider);
            OnExit();
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

    public void OnEnter()
    {
        onEnter.Invoke();
        onInside = true;
    }

    public void OnExit()
    {
        onExit.Invoke();
        if (inContactWith.Count > 0)
            onInside = true;
        else;
            onInside = false;
    }

    public bool HasAlreadyCollide(Collider2D collider)
    {
        bool result = false;
        for(int i = 0; i < inContactWith.Count; i++)
        {
            if (inContactWith[i].collider == collider)
                result = true;
        }
        return result;
    }

    public ObjectCollider GetObjectByCollider(Collider2D collider)
    {
        for (int i = 0; i < inContactWith.Count; i++)
        {
            if (inContactWith[i].collider == collider)
                return inContactWith[i];
        }
        return null;
    }
}
