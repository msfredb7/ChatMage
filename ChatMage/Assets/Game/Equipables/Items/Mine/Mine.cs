using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine {

    public Mine(GameObject mine, float explosionForce = 1)
    {
        mine.GetComponent<Vehicle>().Speed = new Vector2(0,0);
        mine.GetComponent<Vehicle>().TeleportPosition(Game.instance.Player.vehicle.Position);
        mine.GetComponent<MineScript>().SetExplosionForce(explosionForce);
    }
}
