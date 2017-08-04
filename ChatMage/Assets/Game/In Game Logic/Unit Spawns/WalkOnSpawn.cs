using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;


public class WalkOnSpawn : OnSpawnAction
{
    public bool gizmosAlwaysVisible = true;
    public Vector2 destination = new Vector2(3, 2);
    
    public void DrawGizmos()
    {
        if (attachedSpawn == null)
            return;

        Gizmos.color = new Color(0, 0, 1, 0.75f);
        Gizmos.DrawLine(attachedSpawn.transform.position, attachedSpawn.transform.position + (Vector3)destination);
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

    protected override void AttachedSpawn_onUnitSpawned(Unit unit)
    {
        if (!(unit is EnemyVehicle))
            return;

        EnemyVehicle veh = unit as EnemyVehicle;

        //Deactivate brain
        EnemyBrainV2 brain = veh.GetComponent<EnemyBrainV2>();
        if (brain != null)
            brain.enabled = false;

        //On fait marcher l'ennemi vers le centre de la map
        veh.GotoPosition(veh.Position + destination, delegate ()
        {
            //On reactive le cerveau a la fin
            if (brain != null)
                brain.enabled = true;
        });
    }
}
