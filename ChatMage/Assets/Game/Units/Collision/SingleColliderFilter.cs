using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleColliderFilter
{
    /// <summary>
    /// Si 'groupByUnits' est vrai, alors nous allons compter l'entrer d'une unit comme UNE seul entrer,
    ///  meme si elle possede plusieur unitsGroup (ex: comme le shielder et son shield)
    /// </summary>
    /// <param name="groupByUnits"></param>
    public SingleColliderFilter(bool groupByUnits = false)
    {
        this.groupByUnits = groupByUnits;
    }

    public class Contact
    {
        public Unit unit;
        public GameObject groupParent;
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
        public Contact(Unit unit, GameObject head) { this.unit = unit; this.groupParent = head; }
    }

    public LinkedList<Contact> contacts = new LinkedList<Contact>();

    private List<ColliderInfo> forgetList = new List<ColliderInfo>();

    public Action<Unit> onUnitAdded;
    public Action<Unit> onUnitRemoved;
    public bool groupByUnits;

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

        LinkedListNode<Contact> node = GetContactNode(info);
        Contact contact = node != null ? node.Value : null;

        if (contact == null)
        {
            Unit unit = info.parentUnit;
            contacts.AddLast(contact = new Contact(unit, info.GroupParent));
            if (onUnitAdded != null)
                onUnitAdded(unit);
        }

        contact.AddCollision(info);
    }

    public void OnTriggerExit(ColliderInfo info)
    {
        LinkedListNode<Contact> node = GetContactNode(info);
        Contact contact = node != null ? node.Value : null;

        if (contact == null)
        {
            forgetList.Add(info);
        }
        else
        {
            contact.RemoveCollision(info);

            if (contact.CollisionCount <= 0)
            {
                if (onUnitRemoved != null)
                    onUnitRemoved(contact.unit);
                contacts.Remove(node);
            }
        }
    }

    private LinkedListNode<Contact> GetContactNode(ColliderInfo info)
    {
        LinkedListNode<Contact> node = contacts.First;
        while (node != null)
        {
            if ((groupByUnits && node.Value.unit == node.Value.unit)
                || (!groupByUnits && node.Value.groupParent == info.GroupParent))
            {
                return node;
            }

            node = node.Next;
        }
        return null;
    }
}
