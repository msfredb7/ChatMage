
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleUnitSpawn : UnitSpawn
{

    public float minRange = 1;
    public float maxRange = 2;

    public override void DrawGizmos()
    {
        Gizmos.color = new Color(1,0,0,0.75f);
        Gizmos.DrawSphere(transform.position, maxRange);
        Gizmos.color = new Color(0, 0, 0, 0.75f);
        Gizmos.DrawSphere(transform.position, minRange);

        base.DrawGizmos();
    }

    public override Vector2 DefaultSpawnPosition()
    {
        float angle = Random.Range(0, 360);
        Vector2 v = Vehicle.AngleToVector(angle);
        Vector2 deltaPos = v * Random.Range(minRange, maxRange);

        switch (posType)
        {
            default:
            case PositionType.World:
                return (Vector2)transform.position + deltaPos;
            case PositionType.RelativeToPlayer:
                PlayerController player = Game.Instance.Player;
                if (player != null)
                    return player.vehicle.Position + deltaPos;
                else
                    return Game.Instance.gameCamera.Center;
            case PositionType.RelativeToCamera:
                return Game.Instance.gameCamera.Center + deltaPos;
        }
    }

    public override float DefaultSpawnRotation()
    {
        return Vehicle.VectorToAngle(transform.right);
    }
}
