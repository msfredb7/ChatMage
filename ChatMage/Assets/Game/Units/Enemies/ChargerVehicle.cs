using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerVehicle : EnemyVehicle
{
	public void Hit(Unit unit)
    {
        if(unit.gameObject == Game.instance.Player.gameObject)
            Game.instance.Player.GetComponent<PlayerStats>().Hit();
        Destroy(gameObject);
    }
}
