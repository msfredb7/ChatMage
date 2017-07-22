using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SmashBall : MovingUnit, IAttackable
{
    [Header("Smash Ball")]
    public int hp = 3;
    public float startSpeed;
    public Transform followTarget;
    public float followSpeed = 10;
    public float hitSelfImpulse = 7;

    public event SimpleEvent onHitPlayer;
    private bool overReact = false;

    void Start()
    {
        rb.velocity = Vehicle.AngleToVector(UnityEngine.Random.Range(0, 360)) * startSpeed * TimeScale;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (followTarget != null)
        {
            Vector2 v = followTarget.position - tr.position;
            rb.velocity = Vector2.MoveTowards(rb.velocity, v * 2, FixedDeltaTime() * followSpeed);
        }
        if (overReact)
        {
            float mag = Speed.magnitude;
            if (mag < hitSelfImpulse && Speed != Vector2.zero)
                Speed *= hitSelfImpulse / mag;
            overReact = false;
        }
    }

    public void ForceDeath()
    {
        Die();
    }

    public int Attacked(ColliderInfo on, int amount, Unit otherUnit, ColliderInfo source = null)
    {
        hp--;

        if (onHitPlayer != null)
            onHitPlayer();

        if (hp <= 0)
            Die();

        overReact = true;

        return hp;
    }

    public bool CanHit
    {
        get { return allegiance == Allegiance.SmashBall; }
        set
        {
            if (value)
            {
                allegiance = Allegiance.SmashBall;
            }
            else
            {
                allegiance = Allegiance.Neutral;
            }
        }
    }
}
