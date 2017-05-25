using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBrain : EnemyBrain<ProjectileVehicule> {

	protected override void Start ()
    {
        base.Start();
        vehicle.SetDestination();
    }
}
