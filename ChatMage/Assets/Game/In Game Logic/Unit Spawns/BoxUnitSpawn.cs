using UnityEngine;
using System.Collections;

public class BoxUnitSpawn : UnitSpawn
{
    public Vector2 size = Vector2.one;

    public override void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.75f);
        Gizmos.DrawCube(transform.position, size);

        base.OnDrawGizmosSelected();
    }

    Vector2 GetRandomLocation()
    {
        Vector2 position = transform.position;
        float x = (Random.value - 0.5f) * size.x + position.x;
        float y = (Random.value - 0.5f) * size.y + position.y;
        switch (posType)
        {
            default:
            case PositionType.World:
                return new Vector2(x, y);
            case PositionType.RelativeToPlayer:
                PlayerController player = Game.instance.Player;
                if (player != null)
                    return new Vector2(x, y) + player.vehicle.Position;
                else
                    return new Vector2(x, y);
            case PositionType.RelativeToCamera:
                return new Vector2(x, y) + Game.instance.gameCamera.Center;
        }
    }

    public override Vector2 DefaultSpawnPosition()
    {
        return GetRandomLocation();
    }

    public override float DefaultSpawnRotation()
    {
        return Vehicle.VectorToAngle(transform.right);
    }
}
