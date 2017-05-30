using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerBrain : EnemyBrain<ChargerVehicle>
{
    protected override void Start()
    {
        base.Start();
        vehicle.Init();
    }

    public void Update ()
    {
        vehicle.ChargePlayer();
	}
}
