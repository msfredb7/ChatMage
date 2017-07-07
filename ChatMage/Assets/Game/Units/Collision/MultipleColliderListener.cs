using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultipleColliderListener : MonoBehaviour
{
    [Header("Linking")]
    public ColliderListener[] colliderListeners;
    [Header("Settings")]
    public bool useTrigger;
    public bool useCollision;

    void Start()
    {
        int count = colliderListeners.Length;
        for (int i = 0; i < count; i++)
        {
            if (useCollision)
            {
                colliderListeners[i].OnCollisionEnter += OnRemoteCollisionEnter2D;
                colliderListeners[i].OnCollisionExit += OnRemoteCollisionExit2D;
            }
            if (useTrigger)
            {
                colliderListeners[i].OnTriggerEnter += OnRemoteTriggerEnter2D;
                colliderListeners[i].OnTriggerExit += OnRemoteTriggerExit2D;
            }
        }
    }


    [System.Serializable]
    public class ObjectCollider
    {
        public GameObject group;
        public int collisionCount;
        public List<ColliderPair> includedListeners = new List<ColliderPair>();
        public Unit unit;

        [System.Serializable]
        public class ColliderPair
        {
            public ColliderListener a;
            public ColliderInfo b;
            public ColliderPair(ColliderListener a, ColliderInfo b)
            {
                this.a = a;
                this.b = b;
            }
        }

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

    [Header("Dynamic List")]
    public List<ObjectCollider> inContactWith = new List<ObjectCollider>();
    private List<ColliderListener> forgetList = new List<ColliderListener>(3);

    void Update()
    {
        forgetList.Clear();
    }

    public void OnRemoteTriggerEnter2D(ColliderInfo info, ColliderListener listener)
    {
        //Est-ce que l'objet est entré/sortie dans la même frame ?
        int i = forgetList.IndexOf(listener);
        if (i >= 0)
        {
            forgetList.RemoveAt(i);
            return;
        }

        GameObject group = info.GroupParent;
        ObjectCollider obj = GetObjectByGroup(group);

        if (obj == null)
            obj = new ObjectCollider(group, info.parentUnit);

        if (obj.includedListeners.Find(x => x.a == listener && x.b == info) != null)
            return;

        obj.includedListeners.Add(new ObjectCollider.ColliderPair(listener, info));
        

        obj.collisionCount++;
        if (obj.collisionCount == 1)
        {
            inContactWith.Add(obj);
            OnEnter(info, listener);
        }
    }

    public void OnRemoteTriggerExit2D(ColliderInfo info, ColliderListener listener)
    {
        GameObject group = info.GroupParent;
        ObjectCollider obj = GetObjectByGroup(group);

        ObjectCollider.ColliderPair pair = obj.includedListeners.Find(x => x.a == listener && x.b == info);

        if (obj == null || pair == null)
        {
            forgetList.Add(listener);  //L'objet est entré et sortie dans la même frame
        }
        else
        {
            obj.includedListeners.Remove(pair);
            obj.collisionCount--;
            if (obj.collisionCount <= 0)
            {
                inContactWith.Remove(obj);
                OnExit(info, listener);
            }
        }
    }

    public void OnEnter(ColliderInfo otherInfo, ColliderListener listener)
    {
        if (onTriggerEnter != null)
            onTriggerEnter(otherInfo, listener);
    }

    public void OnExit(ColliderInfo otherInfo, ColliderListener listener)
    {
        if (onTriggerExit != null)
            onTriggerExit(otherInfo, listener);
    }

    void OnRemoteCollisionEnter2D(ColliderInfo info, Collision2D collision, ColliderListener listener)
    {
        if (onCollisionEnter != null)
            onCollisionEnter(info, collision, listener);
    }

    void OnRemoteCollisionExit2D(ColliderInfo info, Collision2D collision, ColliderListener listener)
    {
        if (onCollisionExit != null)
            onCollisionEnter(info, collision, listener);
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
