using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerCarZone : MonoBehaviour
{
    public float timescaleMultiplier = 0.5f;

    private SingleColliderFilter filter = new SingleColliderFilter();

    void Start()
    {
        filter.onUnitAdded = Hack;
        filter.onUnitRemoved = Unhack;
    }

    void Hack(Unit unit)
    {
        unit.TimeScale *= timescaleMultiplier;
    }

    void Unhack(Unit unit)
    {
        unit.TimeScale /= timescaleMultiplier;
    }

    void OnDisable()
    {
        foreach (SingleColliderFilter.Contact contact in filter.contacts)
        {
            if (contact.unit != null)
                Unhack(contact.unit);
        }
        filter.Clear();
    }

    void Update()
    {
        filter.RemoteUpdate();
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        ColliderInfo other = col.GetComponent<ColliderInfo>();
        if (other == null)
            return;

        if (other.parentUnit == null || other.parentUnit.allegiance == Allegiance.Ally)
            return;

        filter.OnTriggerEnter(other);
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        ColliderInfo other = col.GetComponent<ColliderInfo>();
        if (other == null)
            return;

        if (other.parentUnit == null || other.parentUnit.allegiance == Allegiance.Ally)
            return;

        filter.OnTriggerExit(other);
    }
}
