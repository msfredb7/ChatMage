using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JesusV2Brain : EnemyBrain<JesusV2Vehicle>
{

    protected override void UpdateWithoutTarget()
    {
        if (CanGoTo<WanderBehavior>())
            SetBehavior(new WanderBehavior(vehicle));
    }
    protected override void UpdateWithTarget()
    {
        //Si nous ne sommes pas entrain de chercher une roche OU de la lancer  ->  aller checker une roche
        if (!IsBehavior<JesusSearchRockBehavior>() && !IsBehavior<JesusRockAttackBehavior>())
        {
            SetBehavior(new JesusSearchRockBehavior(vehicle, RockReadyToPickup));
        }
    }

    private void RockReadyToPickup(JesusRockV2 rock)
    {
        SetBehavior(new JesusRockAttackBehavior(vehicle, rock, RockThrown));
    }

    private void RockThrown()
    {
        SetBehavior(new JesusSearchRockBehavior(vehicle, RockReadyToPickup));
    }
}
