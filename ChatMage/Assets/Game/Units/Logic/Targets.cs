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

    public Unit TryToFindTarget(Unit myUnit, bool searchOnlyIAttackables = true)
    {
        if (targetAllegiances.Count == 0)
            return null;

        //En g�n�ral, on passe ici, sachant que les ennemi cherche pas mal toujours le joueur
        if (targetAllegiances.Count == 1 && targetAllegiances[0] == Allegiance.Ally)
        {
            PlayerController player = Game.instance == null ? null : Game.instance.Player;

            if (player != null && Unit.HasPresence(player.vehicle))
                return player.vehicle;
            else
                return null;
        }
        else
        {
            //Cherche a travers tous les units pour trouver la plus pret

            Vector2 myPos = myUnit.Position;
            float smallestDistance = float.PositiveInfinity;
            Unit recordHolder = null;

            LinkedList<Unit> list = searchOnlyIAttackables ? Game.instance.attackableUnits : Game.instance.units;

            foreach (Unit unit in list)
            {
                if (unit == myUnit)
                    continue;

                if (Unit.HasPresence(unit) && IsValidTarget(unit))
                {
                    float sqrDistance = (unit.Position - myPos).sqrMagnitude;
                    if (sqrDistance < smallestDistance)
                    {
                        smallestDistance = sqrDistance;
                        recordHolder = unit;
                    }
                }
            }
            return recordHolder;
        }
    }
}
