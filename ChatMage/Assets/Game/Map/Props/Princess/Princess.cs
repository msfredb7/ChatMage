using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Princess : Unit, IAttackable
{

    [Header("Linking"), Forward]
    public Targets targets;

    public string saveDialogEvent = "saveDialogEvent";

    void Start()
    {
    
    }

    //Si le shield se fait frapper, il resiste et tourne
    public int Attacked(ColliderInfo on, int amount, Unit otherUnit, ColliderInfo source = null)
    {
        if (otherUnit is PlayerVehicle)
        {
            // TRIGGER LES EVENNEMENTS DE PRENDRE LA PRINCESSE
            // Animation ?
            List<IMilestone> milestones = Game.instance.map.roadPlayer.CurrentRoad.milestones;
            for (int i = 0; i < milestones.Count; i++)
            {
                milestones[i].GameObj.SetActive(true);
            }

            if (!IsDead)
                Die();
            return 0;
        }
        else
        {
            return 1;
        }
    }

    protected override void Die()
    {
        Game.instance.levelScript.ReceiveEvent(saveDialogEvent);

        base.Die();

        Destroy();
    }

    public int SmashJuice()
    {
        return 0;
    }
}

