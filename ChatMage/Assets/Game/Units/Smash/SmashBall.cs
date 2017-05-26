using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashBall : Unit
{
    public int hp = 3;
    public float startSpeed;
    public Transform followTarget;
    public float followSpeed = 10;

    public event SimpleEvent onHitPlayer;

    void Start()
    {
        rb.velocity = Vehicle.AngleToVector(Random.Range(0, 360)) * startSpeed;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if(followTarget != null)
        {
            Vector2 v = followTarget.position - tr.position;
            rb.velocity = Vector2.MoveTowards(rb.velocity, v * 2, FixedDeltaTime() * followSpeed);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        ColliderInfo info = collision.collider.GetComponent<ColliderInfo>();
        if (info == null)
            return;

        if (info.parentUnit != Game.instance.Player.vehicle)
            return;

        //Is player !
        OnCollisionWithPlayer();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        ColliderInfo info = other.GetComponent<ColliderInfo>();
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
        if (onHitPlayer != null)
            onHitPlayer();

        if (hp <= 0)
            Die();
    }

    protected override void Die()
    {
        //Call base event
        base.Die();


        //Death animation !
        Destroy(gameObject);
    }
}
