using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Math;

public class ArcherRepositionBehavior : EnemyBehavior<ArcherVehicle>
{
    private const float MIN_DIST = 1.5f;
    private const float MAX_DIST = 3f;
    private const float TOO_CLOSE_DIST = 10; //CETTE DISTANCE EST ^2

    public ArcherRepositionBehavior(ArcherVehicle vehicle)
        : base(vehicle)
    {

    }

    public override void Enter(Unit target)
    {
        Vector2 deltaMove = Vector2.zero;
        if (target == null)
        {
            deltaMove = Vectors.RandomVector2(0, 360, MIN_DIST, MAX_DIST);
        }
        else
        {
            Vector2 meToTarget = target.Position - vehicle.Position;
            float angleToTarget = Vectors.VectorToAngle(meToTarget);

            bool invert = UnityEngine.Random.value > 0.5f;

            if (meToTarget.sqrMagnitude < TOO_CLOSE_DIST)
            {
                deltaMove = Vectors.RandomVector2(angleToTarget + 90, angleToTarget + 120, MIN_DIST, MAX_DIST);
            }
            else
            {
                deltaMove = Vectors.RandomVector2(angleToTarget + 60, angleToTarget + 120, MIN_DIST, MAX_DIST);
            }
            if (invert)
                deltaMove = -deltaMove;

            //But: Donner ceci
            //____________>>>>>_____________________<<<<<__________________
            //____________>>>>>>>>>>___________<<<<<<<<<<__________________
            //____________>>>>>>>>>>>>>>>O<<<<<<<<<<<<<<<__________________
            //____________>>>>>>>>>>___________<<<<<<<<<<__________________
            //____________>>>>>_____________________<<<<<__________________
        }

        vehicle.GotoPosition(deltaMove + vehicle.Position);
    }

    public override void Exit(Unit target)
    {

    }

    public override void Update(Unit target, float deltaTime)
    {

    }
}
