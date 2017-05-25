using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileVehicule : Unit {

    public float speed;

    public void Hit(Unit unit)
    {
        if (unit.gameObject == Game.instance.Player.gameObject)
        {
            Destroy(gameObject);
            Game.instance.Player.playerStats.Hit();
        }
    }

    public void Start()
    {
        if (Game.instance.Player != null)
            rb.velocity = (Game.instance.Player.vehicle.Position - Position).normalized * speed;
        GetComponent<SimpleCollisionListener>().onTriggerEnter += Hit;
    }

}
