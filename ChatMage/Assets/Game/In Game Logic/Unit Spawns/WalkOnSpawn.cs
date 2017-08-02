using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;


public class WalkOnSpawn : OnSpawnAction
{
    public Vector2 destination = new Vector2(3, 2);
    
    public void OnDrawGizmosSelected()
    {
        if (attachedSpawn == null)
            return;

        Gizmos.color = new Color(0, 0, 1, 0.75f);
        Gizmos.DrawLine(attachedSpawn.transform.position, attachedSpawn.transform.position + (Vector3)destination);
    }

    protected override void AttachedSpawn_onUnitSpawned(Unit unit)
    {
        if (!(unit is EnemyVehicle))
            return;

        EnemyVehicle veh = unit as EnemyVehicle;

        //Deactivate brain
        EnemyBrain brain = veh.GetComponent<EnemyBrain>();
        if (brain != null)
            brain.enabled = false;

        EnemyBrainV2 brainV2 = veh.GetComponent<EnemyBrainV2>();
        if (brainV2 != null)
            brainV2.enabled = false;

        //On fait marcher l'ennemi vers le centre de la map
        veh.GotoPosition(veh.Position + destination, delegate ()
        {
            //On reactive le cerveau a la fin
            if (brain != null)
                brain.enabled = true;
            if (brainV2 != null)
                brainV2.enabled = true;
        });
    }
}
