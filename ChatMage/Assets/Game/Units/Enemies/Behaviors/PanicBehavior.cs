using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanicBehavior : EnemyBehavior<EnemyVehicle>
{
    const float MIN_DIST = 1.5f;
    const float MAX_DIST = 3;

    public PanicBehavior(EnemyVehicle v) : base(v) { }

    public override void Enter(Unit target)
    {
        NewDestination();
    }

    private void NewDestination()
    {
        vehicle.GotoPosition(CCC.Math.Vectors.RandomVector2(0, 360, MIN_DIST, MAX_DIST) + vehicle.Position, NewDestination);
    }

    public override void Exit(Unit target)
    {
    }

    public override void Update(Unit target, float deltaTime)
    {
    }
}
