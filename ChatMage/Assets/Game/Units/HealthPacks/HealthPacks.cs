using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPacks : MovingUnit {

	public void PickUp(Unit unit)
    {
        if (unit == Game.instance.Player.vehicle)
            Game.instance.Player.GetComponent<PlayerStats>().Regen();
        Destroy(gameObject);
    }
}
