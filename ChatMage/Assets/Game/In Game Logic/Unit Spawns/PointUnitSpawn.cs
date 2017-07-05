using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointUnitSpawn : UnitSpawn
{
    public override void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.75f);
        Gizmos.DrawSphere(transform.position, 0.35f);
        base.OnDrawGizmosSelected();
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
                PlayerController player = Game.instance.Player;
                if (player != null)
                    return position + player.vehicle.Position;
                else
                    return position;
            case PositionType.RelativeToCamera:
                return position + Game.instance.gameCamera.Center;
        }
    }

    public override float DefaultSpawnRotation()
    {
        return Vehicle.VectorToAngle(transform.right);
    }
}
