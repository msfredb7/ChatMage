using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerBrain : EnemyBrain<ChargerVehicle>
{
	public void Update ()
    {
        vehicle.ChargePlayer();
	}
}
