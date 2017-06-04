using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanicBehavior : EnemyBehavior<EnemyVehicle>
{
    const float CHOOSE_INTERVAL = 1.5f;

    float chooseTimer = 0;
    float lastDirectionPick = 0;

    public PanicBehavior(EnemyVehicle v) : base(v) { }

    public override BehaviorType Type { get { return BehaviorType.Panic; } }

    public override void Enter(PlayerController player)
    {
    }

    public override void Exit(PlayerController player)
    {
    }

    public override void Update(PlayerController player, float deltaTime)
    {
        chooseTimer -= deltaTime;

        //Pick new destination ?
        if (chooseTimer < 0)
        {
            // on reste sous 360
            if (lastDirectionPick > 360)
                lastDirectionPick -= 360;

            //On met cette petite formule pour ne pas pick 2 fois des direction semblable
            vehicle.GotoDirection(lastDirectionPick = Random.Range(lastDirectionPick + 60, lastDirectionPick + 300), deltaTime);
            chooseTimer = CHOOSE_INTERVAL;
        }
    }
}
