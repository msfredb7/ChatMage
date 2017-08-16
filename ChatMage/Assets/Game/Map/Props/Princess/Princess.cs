using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Princess : Unit, IAttackable
{

    [Header("Linking"), Forward]
    public Targets targets;

    public string saveDialogEvent;

    void Start()
    {
    
    }

    //Si le shield se fait frapper, il resiste et tourne
    public int Attacked(ColliderInfo on, int amount, Unit otherUnit, ColliderInfo source = null)
    {
        if (otherUnit is PlayerVehicle)
        {
            // TRIGGER LES EVENNEMENTS DE PRENDRE LA PRINCESSE

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
}

