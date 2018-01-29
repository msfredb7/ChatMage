using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.DesignPattern;

public class MarkerSpawner : Pool<Marker>
{
    public Marker markerPrefab;
    public int count;

    void Update()
    {
        count = deactivatedPool.Count;
    }

    public Marker Mark(Unit unit, Transform aimAt)
    {
        Marker m = Mark(unit);
        m.SetLookAtTarget(aimAt);
        return m;
    }

    public Marker Mark(Unit unit, MarkerReceiver aimAt)
    {
        Marker m = Mark(unit);
        m.SetLookAtTarget(aimAt);
        return m;
    }

    private Marker Mark(Unit unit)
    {
        Marker m = GetFromPool();
        m.DeployOn(unit);
        return m;
    }

    protected override Marker NewItem()
    {
        return Instantiate(markerPrefab.gameObject, transform).GetComponent<Marker>();
    }
}
