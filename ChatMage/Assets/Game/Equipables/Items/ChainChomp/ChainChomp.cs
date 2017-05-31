using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainChomp : Unit
{
    public Rigidbody2D anchor;
    public Rigidbody2D followingBall;
    public Rigidbody2D realBall;
    public SimpleColliderListener colliderListener;
    public int hitDamage = 1;
    public GameObject container;
    public float drag = 2;

    private bool teleported = false;
    private Transform target;
    private float lastVerticalSpeed;
    private float parentVelocity;

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
        if (other.parentUnit.allegiance == Allegiance.Enemy)
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
                attackable.Attacked(other, hitDamage * Game.instance.Player.playerStats.damageMultiplier, this);
        }
    }

    protected override void FixedUpdate()
    {
        //Artificial drag
        if (useMovingPlatform && Game.instance.map.rubanPlayer != null)
        {
            parentVelocity = Game.instance.map.rubanPlayer.GetVerticalSpeed();
            float delta = parentVelocity - lastVerticalSpeed;

            Vector2 totalVel = realBall.velocity - Vector2.up * parentVelocity;
            realBall.AddForce(totalVel.sqrMagnitude * drag * -totalVel.normalized * TimeScale, ForceMode2D.Force);

            realBall.velocity += delta * Vector2.up;

            lastVerticalSpeed = parentVelocity;
        }

        if (!container.activeSelf)
        {
            if (teleported)
                teleported = false;
            else
                container.SetActive(true);
        }

        if (target != null)
            anchor.MovePosition(target.position);

        followingBall.MovePosition(realBall.position);
    }

    void OnPlayerTeleport(Unit player, Vector2 delta)
    {
        container.SetActive(false);
        container.transform.position += new Vector3(delta.x, delta.y);
        teleported = true;
    }
}