using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPacks : MovingUnit {

    private void Start()
    {
        GetComponentInChildren<CollisionListener>().onEnter += PickUp;
    }

    public void PickUp(Unit unit)
    {
        if (unit == Game.instance.Player.vehicle)
            Game.instance.Player.GetComponent<PlayerStats>().Regen();
        Destroy(gameObject);
    }
}
