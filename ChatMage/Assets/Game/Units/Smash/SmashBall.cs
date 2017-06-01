using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashBall : Unit, IAttackable
{
    public int hp = 3;
    public float startSpeed;
    public Transform followTarget;
    public float followSpeed = 10;

    public event SimpleEvent onHitPlayer;

    void Start()
    {
        rb.velocity = Vehicle.AngleToVector(UnityEngine.Random.Range(0, 360)) * startSpeed;
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

    public void ForceDeath()
    {
        Die();
    }

    protected override void Die()
    {
        //Call base event
        base.Die();


        //Death animation !
        Destroy(gameObject);
    }

    public int Attacked(ColliderInfo on, int amount, MonoBehaviour source)
    {
        hp--;
        if (onHitPlayer != null)
            onHitPlayer();

        if (hp <= 0)
            Die();

        return hp;
    }
}
