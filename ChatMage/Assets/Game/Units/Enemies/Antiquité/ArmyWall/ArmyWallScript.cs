using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyWallScript : MovingUnit
{
    public Vector2 moveSpeed = new Vector2(0,1);
    public float hitPlayerCooldown = 0.5f;
    public float bumpStrength;
    public float bumpDuration = 0f;
    public new Collider2D collider;
    public float maxYPos = 40;

    public SimpleColliderListener colliderListener;
    private float hitTimer;

    void Start()
    {
        colliderListener.onCollisionEnter += ColliderListener_onCollisionEnter;
    }

    private void ColliderListener_onCollisionEnter(ColliderInfo other, Collision2D collision, ColliderListener listener)
    {
        if (other.parentUnit is PlayerVehicle)
        {
            if (hitTimer <= 0)
            {
                Game.instance.Player.playerStats.Attacked(other, 1, this);
                (other.parentUnit as PlayerVehicle).Bump(WorldDirection2D() * bumpStrength, bumpDuration, BumpMode.VelocityAdd);
                hitTimer = hitPlayerCooldown;
            }
        }
        else if (other.parentUnit is EnemyVehicle)
            (other.parentUnit as EnemyVehicle).Attacked(other, 10, this, listener.info);
    }

    void OnEnable()
    {
        Speed = moveSpeed;
    }

    public void BringCloseToPlayer()
    {
        tr.position = Vector2.up * Game.instance.gameCamera.Bottom;
    }

    public void DisableCollision()
    {
        collider.enabled = false;
    }

    public void EnableCollision()
    {
        collider.enabled = true;
    }

    protected override void Update()
    {
        base.Update();

        Vector2 pos = tr.position;

        tr.position = new Vector3(pos.x, pos.y.Capped(maxYPos), 0);

        if (hitTimer > 0)
            hitTimer -= DeltaTime();
    }

    protected override void Die()
    {
        base.Die();

        Destroy();
    }
}
