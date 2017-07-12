using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Camion : StdCar
{
    public override void Init(PlayerController player)
    {
        base.Init(player);
        player.transform.localScale *= 1.5f;
    }
}
