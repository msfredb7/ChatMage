using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollBrain : EnemyBrain<TrollVehicle>
{
    public JesusRockV2 myRock;

    protected override void Start()
    {
        base.Start();

        myRock.PickedUpState();
        Game.instance.AddExistingUnit(myRock);
        SetBehavior(new TrollRockAttackBehavior(vehicle, myRock, RockThrown, true));
    }

    protected override void UpdateWithoutTarget()
    {
        if (CanGoTo<WanderBehavior>())
            SetBehavior(new WanderBehavior(vehicle));
    }
    protected override void UpdateWithTarget()
    {
        //Si nous ne sommes pas entrain de chercher une roche OU de la lancer  ->  aller checker une roche
        if (myRock == null && !IsBehavior<TrollSearchRockBehavior>() && !IsBehavior<TrollRockAttackBehavior>())
        {
            SetBehavior(new TrollSearchRockBehavior(vehicle, myRock, RockReadyToPickup));
        }
    }

    private void RockReadyToPickup(JesusRockV2 rock)
    {
        SetBehavior(new TrollRockAttackBehavior(vehicle, rock, RockThrown, false));
    }

    private void RockThrown()
    {
        SetBehavior(new TrollSearchRockBehavior(vehicle, myRock, RockReadyToPickup));
    }
}
