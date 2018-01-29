using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotoOnSpawn : OnSpawnAction
{
    public bool gizmosAlwaysVisible = true;
    public UnitSpawn.PositionType positionType;
    public Vector2 delta = new Vector2(3, 2);
    public bool setUnitDirection = false;


    public void DrawGizmos()
    {
        if (attachedSpawn == null)
            return;

        Gizmos.color = new Color(1, 1, 0, 0.75f);
        Gizmos.DrawSphere(Destination, 0.25f);
        Gizmos.color = new Color(0, 0, 1, 0.75f);
        Gizmos.DrawLine(attachedSpawn.transform.position, Destination);
    }

    void OnDrawGizmosSelected()
    {
        if (!gizmosAlwaysVisible)
            DrawGizmos();
    }

    void OnDrawGizmos()
    {
        if (gizmosAlwaysVisible)
            DrawGizmos();
    }

    Vector2 Destination { get { return (Vector2)transform.position + delta; } }

    protected override void AttachedSpawn_onUnitSpawned(Unit unit)
    {
        if (!(unit is EnemyVehicle))
            return;

        EnemyVehicle veh = unit as EnemyVehicle;

        //Deactivate brain
        AI.EnemyBrainV2 brain = veh.GetComponent<AI.EnemyBrainV2>();
        if (brain != null)
            brain.enabled = false;

        Vector2 destination = Destination;

        switch (positionType)
        {
            default:
            case UnitSpawn.PositionType.World:
                break;
            case UnitSpawn.PositionType.RelativeToPlayer:
                PlayerController player = Game.Instance.Player;
                if (player != null)
                    destination += player.vehicle.Position;
                else
                    destination += Game.Instance.gameCamera.Center;
                break;
            case UnitSpawn.PositionType.RelativeToCamera:
                destination += Game.Instance.gameCamera.Center;
                break;
        }

        if (setUnitDirection)
        {
            //On fait tourner l'unite vers la destination
            Vector2 deltaPos = destination - veh.Position;
            veh.TeleportDirection(Vehicle.VectorToAngle(deltaPos.normalized));
        }

        //On fait marcher l'ennemi vers le centre de la map
        veh.GotoPosition(destination, delegate ()
        {
            //On reactive le cerveau a la fin
            if (brain != null)
                brain.enabled = true;
        });
    }
}
