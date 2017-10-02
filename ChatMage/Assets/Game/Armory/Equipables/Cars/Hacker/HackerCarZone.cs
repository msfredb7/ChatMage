using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerCarZone : MonoBehaviour
{
    public float timescaleMultiplier = 0.5f;

    private SingleColliderFilter filter = new SingleColliderFilter(true);

    void Start()
    {
        filter.onUnitAdded = Hack;
        filter.onUnitRemoved = Unhack;
    }

    public void SetSizeMultiplier(float multiplier = 1)
    {
        transform.localScale = Vector3.one * multiplier;
    }

    void Hack(Unit unit)
    {
        unit.TimeScale *= timescaleMultiplier;
    }

    void Unhack(Unit unit)
    {
        if (!unit.IsDead)
            unit.TimeScale /= timescaleMultiplier;
    }

    void OnDisable()
    {
        foreach (SingleColliderFilter.Contact contact in filter.contacts)
        {
            if (contact.unit != null && !contact.unit.IsDead)
                Unhack(contact.unit);
        }
        filter.Clear();
    }

    void Update()
    {
        filter.RemoteUpdate();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        ColliderInfo other = col.GetComponent<ColliderInfo>();
        if (other == null)
            return;

        if (other.parentUnit == null || other.parentUnit.allegiance == Allegiance.Ally)
            return;

        filter.OnTriggerEnter(other);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        ColliderInfo other = col.GetComponent<ColliderInfo>();
        if (other == null)
            return;

        if (other.parentUnit == null || other.parentUnit.allegiance == Allegiance.Ally)
            return;

        filter.OnTriggerExit(other);
    }
}
