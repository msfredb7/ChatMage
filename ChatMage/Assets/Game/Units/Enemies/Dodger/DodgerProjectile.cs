using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgerProjectile : Unit {

    public float speed;

    public void Hit(ColliderInfo info, ColliderListener listener)
    {
        if (info.parentUnit.gameObject == Game.instance.Player.gameObject)
        {
            Game.instance.Player.playerStats.Attacked(info, 1, this);
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        if (Game.instance.Player != null)
            rb.velocity = (Game.instance.Player.vehicle.Position - Position).normalized * speed;
        GetComponent<SimpleColliderListener>().onTriggerEnter += Hit;
    }

    public void Kill()
    {
        Die();
    }

    protected override void Die()
    {
        base.Die();
        Destroy(gameObject);
    }

}
