using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Targets
{
    public List<Allegiance> targetAllegiances;
    public event SimpleEvent onTargetAdded;
    public event SimpleEvent onTargetRemoved;

    public Targets()
    {
        targetAllegiances = new List<Allegiance>();
    }
    public Targets(Targets copy)
    {
        targetAllegiances = new List<Allegiance>(copy.targetAllegiances);
    }
    public Targets(params Allegiance[] targetAllegiances)
    {
        this.targetAllegiances = new List<Allegiance>(targetAllegiances);
    }

    public bool IsValidTarget(Allegiance allegiance)
    {
        if (targetAllegiances == null)
            return false;

        for (int i = 0; i < targetAllegiances.Count; i++)
        {
            if (allegiance == targetAllegiances[i])
                return true;
        }
        return false;
    }

    public bool IsValidTarget(Unit unit)
    {
        return IsValidTarget(unit.allegiance);
    }

    public void AddTargetAllegiance(Allegiance targetAllegiance)
    {
        if (!IsValidTarget(targetAllegiance))
        {
            targetAllegiances.Add(targetAllegiance);

            //Event
            if (onTargetAdded != null)
                onTargetAdded();
        }
    }

    public void RemoveTargetAllegiance(Allegiance targetAllegiance)
    {
        if (targetAllegiances.Remove(targetAllegiance))
        {
            //Event
            if (onTargetRemoved != null)
                onTargetRemoved();
        }
    }
}
