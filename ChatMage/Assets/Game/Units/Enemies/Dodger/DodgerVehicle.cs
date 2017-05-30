using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgerVehicle : EnemyVehicle
{
    public GameObject visualAspect; // sprite qui regarde vers ou on tire
    private float shootCooldown = 1;

    public DodgerProjectile projectilePrefab;

    private DodgerProjectile lastProjectile;

    public void Init()
    {
        SetBounds(Game.instance.ScreenBounds, 1);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        shootCooldown -= FixedDeltaTime();
    }

    public bool CanShoot()
    {
        return shootCooldown < 0;
    }

    public void DodgeLeft()
    {
        GotoDirection(VectorToAngle(GetPlayerPosition() - GetPosition()) + 90);
    }

    public void DodgeRight()
    {
        GotoDirection(VectorToAngle(GetPlayerPosition() - GetPosition()) - 90);
    }

    public void LookAtPlayer()
    {
        // Rotation du sprite vers le joueur
        visualAspect.transform.rotation = Quaternion.Euler(0, 0, VectorToAngle(GetPlayerPosition() - GetPosition()));
    }

    public Vector2 GetPlayerPosition()
    {
        return new Vector2(Game.instance.Player.transform.position.x, Game.instance.Player.transform.position.y);
    }

    public Vector2 GetPosition()
    {
        return new Vector2(tr.position.x, tr.position.y);
    }

    public void Shoot()
    {
        if (lastProjectile != null)
            lastProjectile.Kill();

        Vector3 spawnPosition = transform.position;

        lastProjectile = Game.instance.SpawnUnit(projectilePrefab, spawnPosition);

        shootCooldown = 5;
    }

    protected override void Die()
    {
        base.Die();

        Destroy(gameObject);
    }

    public override int Attacked(ColliderInfo on, int amount, MonoBehaviour source)
    {
        Die();
        return 0;
    }
}
