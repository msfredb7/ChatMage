using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitDetection
{
    public static List<Unit> OverlapCircleUnits(Vector2 center, float radius, Unit excludedUnit, Allegiance[] allowedAllegiances)
    {
        if (Game.instance == null)
            return null;
        
        List<Unit> filteredList = new List<Unit>(5);

        float sqrRadius = radius * radius;

        
        foreach (Unit unit in Game.instance.units)
        {
            //Unit
            if (unit == excludedUnit)
                continue;

            //Allegiance
            bool skip = true;
            for (int u = 0; u < allowedAllegiances.Length; u++)
            {
                if (unit.allegiance == allowedAllegiances[u])
                {
                    skip = false;
                    break;
                }
            }
            if (skip)
                continue;

            //Ok !
            Vector2 v = unit.Position - center;
            if (v.sqrMagnitude <= sqrRadius)
            {
                filteredList.Add(unit);
            }
        }

        return filteredList;
    }
    public static List<ColliderInfo> OverlapCircleAll(Vector2 point, float radius, LayerMask mask)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(point, radius, mask);

        return GetCollidersInfo(colliders);
    }
    private static List<ColliderInfo> GetCollidersInfo(Collider2D[] colliders)
    {
        List<ColliderInfo> list = new List<ColliderInfo>(colliders.Length);
        List<GameObject> hitGroups = new List<GameObject>(colliders.Length);

        for (int i = 0; i < colliders.Length; i++)
        {
            ColliderInfo info = colliders[i].GetComponent<ColliderInfo>();
            if (info == null)
                continue;

            if (info.groupParent == null)
                list.Add(info);
            else
            {
                if (!hitGroups.Contains(info.groupParent))
                {
                    hitGroups.Add(info.groupParent);
                    list.Add(info);
                }
            }
        }
        return list;
    }
}
