using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Hacker : StdCar
{
    public HackerCarZone zonePrefab;

    public override void Init(PlayerController player)
    {
        base.Init(player);

        Instantiate(zonePrefab.gameObject, player.transform);
    }
}
