using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPacks : MovingUnit {

    private void Start()
    {
        GetComponent<SimpleColliderListener>().onTriggerEnter += PickUp;
    }

    public void PickUp(ColliderInfo info, ColliderListener listener)
    {
        if (info.parentUnit == Game.instance.Player.vehicle)
            Game.instance.Player.playerStats.Regen();
        Die();
    }

    protected override void Die()
    {
        base.Die();

        Destroy(gameObject);
    }
}
