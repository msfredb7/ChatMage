using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointUnitSpawn : UnitSpawn
{
    public override void DrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.75f);
        Gizmos.DrawSphere(transform.position, 0.35f);
        base.DrawGizmos();
    }
    public override Vector2 DefaultSpawnPosition()
    {
        Vector2 position = transform.position;
        switch (posType)
        {
            default:
            case PositionType.World:
                return position;
            case PositionType.RelativeToPlayer:
                PlayerController player = Game.Instance.Player;
                if (player != null)
                    return position + player.vehicle.Position;
                else
                    return position;
            case PositionType.RelativeToCamera:
                return position + Game.Instance.gameCamera.Center;
        }
    }

    public override float DefaultSpawnRotation()
    {
        return Vehicle.VectorToAngle(transform.right);
    }
}
