using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine {

    public Mine(GameObject mine, float explosionForce = 1)
    {
        mine.GetComponent<Vehicle>().Speed = new Vector2(0,0);
        mine.GetComponent<MineScript>().SetMinePosition(Game.instance.Player.vehicle.transform.position);
        mine.GetComponent<MineScript>().SetExplosionForce(explosionForce);
    }
}
