using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashBall : Unit
{
    public int hp = 3;
    public float startSpeed;
    
    public event SimpleEvent onHitPlayer;

    void Start()
    {
        rb.velocity = Vehicle.AngleToVector(Random.Range(0, 360)) * startSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        ColliderInfo info = collision.otherCollider.GetComponent<ColliderInfo>();
        if (info == null)
            return;

        if (info.parentUnit != Game.instance.Player.vehicle)
            return;

        //Is player !
        OnCollisionWithPlayer();
    }

    void OnCollisionWithPlayer()
    {
        hp--;
        onHitPlayer();
    }
}
