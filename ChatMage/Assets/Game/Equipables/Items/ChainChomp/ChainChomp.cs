using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainChomp : Unit
{
    [Header("Chain Chomp")]
    public Rigidbody2D anchor;
    public Rigidbody2D followingBall;
    public Rigidbody2D realBall;
    public Rigidbody2D fistBallLink;
    public SimpleColliderListener colliderListener;
    public int hitDamage = 1;
    public GameObject container;

    private bool teleported = false;
    private Transform target;
    private Vector2 oldBallVel = Vector2.zero;

    protected override void Awake()
    {
        base.Awake();
        rb = realBall;
    }

    public void Init(Transform target, PlayerController player)
    {
        colliderListener.onCollisionEnter += ColliderListener_onCollisionEnter;
        player.vehicle.onTeleportPosition += OnPlayerTeleport;
        player.vehicle.onDestroy += OnPlayerDestroyed;

        this.target = target;
    }

    void OnPlayerDestroyed(Unit unit)
    {
        Destroy(gameObject);
    }

    private void ColliderListener_onCollisionEnter(ColliderInfo other, Collision2D collision, ColliderListener listener)
    {
        if (other.parentUnit.allegiance == Allegiance.Enemy || other.parentUnit.allegiance == Allegiance.SmashBall)
        {
            //Bump !
            if (other.parentUnit is Vehicle)
            {
                if (other.parentUnit.rb.bodyType == RigidbodyType2D.Dynamic)
                    (other.parentUnit as Vehicle).Bump(
                        (other.parentUnit.rb.position - realBall.position).normalized * realBall.velocity.magnitude * 1.5f,
                        0,
                        BumpMode.VelocityAdd);
                else
                    (other.parentUnit as Vehicle).Bump(
                        (other.parentUnit.rb.position - realBall.position).normalized * realBall.velocity.magnitude * 1.5f,
                        0.25f,
                        BumpMode.VelocityAdd);
            }

            IAttackable attackable = other.parentUnit.GetComponent<IAttackable>();
            if (attackable != null)
                attackable.Attacked(other, hitDamage * Game.instance.Player.playerStats.damageMultiplier, this, listener.info);
        }
    }

    protected override void FixedUpdate()
    {
        if (!container.activeSelf)
        {
            if (teleported)
                teleported = false;
            else
                container.SetActive(true);
        }

        if (target != null)
            anchor.MovePosition(target.position);

        float angle = 0;
        float strength = 0.05f * Math.Min(realBall.velocity.sqrMagnitude, 2);

        angle = Vehicle.VectorToAngle(realBall.position - fistBallLink.position) + 90;

        realBall.rotation = Mathf.LerpAngle(realBall.rotation, angle, FixedLerp.FixedFix(strength));


        followingBall.MovePosition(realBall.position);
        oldBallVel = realBall.velocity;
    }

    void OnPlayerTeleport(Unit player, Vector2 delta)
    {
        container.SetActive(false);
        container.transform.position += new Vector3(delta.x, delta.y);
        teleported = true;
        rb.velocity = Vector3.zero;
    }
}