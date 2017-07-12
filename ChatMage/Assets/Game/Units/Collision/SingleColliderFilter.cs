using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleColliderFilter
{
    public class Contact
    {
        public Unit unit;
        public void AddCollision(ColliderInfo other)
        {
            if (!infos.Contains(other))
                infos.AddLast(other);
        }
        public void RemoveCollision(ColliderInfo other)
        {
            infos.Remove(other);
        }
        public int CollisionCount { get { return infos.Count; } }
        private LinkedList<ColliderInfo> infos = new LinkedList<ColliderInfo>();
        public Contact(Unit unit) { this.unit = unit; }
    }

    public LinkedList<Contact> contacts = new LinkedList<Contact>();

    private List<ColliderInfo> forgetList = new List<ColliderInfo>();

    public Action<Unit> onUnitAdded;
    public Action<Unit> onUnitRemoved;

    public void Clear()
    {
        forgetList.Clear();
        contacts.Clear();
    }

    public void RemoteUpdate()
    {
        forgetList.Clear();
    }

    public void OnTriggerEnter(ColliderInfo info)
    {
        for (int i = 0; i < forgetList.Count; i++)
        {
            if (info == forgetList[i])
                return;
        }

        Contact hack = null;
        Unit unit = info.parentUnit;

        LinkedListNode<Contact> node = contacts.First;
        while (node != null)
        {
            if (node.Value.unit == unit)
            {
                hack = node.Value;
                break;
            }

            node = node.Next;
        }

        if (hack == null)
        {
            contacts.AddLast(hack = new Contact(unit));
            if (onUnitAdded != null)
                onUnitAdded(unit);
        }

        hack.AddCollision(info);
    }

    public void OnTriggerExit(ColliderInfo other)
    {
        Contact hack = null;

        LinkedListNode<Contact> node = contacts.First;
        while (node != null)
        {
            if (node.Value.unit == other.parentUnit)
            {
                hack = node.Value;
                break;
            }

            node = node.Next;
        }

        if (hack == null)
        {
            forgetList.Add(other);
        }
        else
        {
            hack.RemoveCollision(other);

            if (hack.CollisionCount <= 0)
            {
                if (onUnitRemoved != null)
                    onUnitRemoved(node.Value.unit);
                contacts.Remove(node);
            }
        }
    }
}
