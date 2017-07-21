using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targets : MonoBehaviour
{
    public List<Allegiance> targetAllegiances;

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
        }
    }
}
