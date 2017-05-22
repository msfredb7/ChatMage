using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerBrain : EnemyBrain
{
	public void Update ()
    {
        mySelf.vehicle.ChargePlayer();
	}
}
