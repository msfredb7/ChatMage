using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerBrain : EnemyBrain
{
	public override void Update ()
    {
        mySelf.vehicle.ChargePlayer();
	}
}
