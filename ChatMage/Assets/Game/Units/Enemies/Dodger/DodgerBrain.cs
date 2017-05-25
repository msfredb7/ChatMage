using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgerBrain : EnemyBrain<DodgerVehicle>
{
    public float minDodgeDelay = 1;
    public float maxDodgeDelay = 2;
    public float minShootDelay = 1;
    public float maxShootDelay = 2;

    public enum Direction
    {
        right = 0,
        left = 1
    }

    public Direction currentDirection = Direction.left;

    public GameObject projectile;

    protected override void Start()
    {
        base.Start();
        vehicle.Init();
        ChangeDirection();
        ThrowProjectile();
    }

    public void Update()
    {
        if (Game.instance.Player == null)
            return;
        vehicle.LookAtPlayer();
    }

    void ChangeDirection()
    {
        if (Game.instance.Player == null)
            return;

        switch (currentDirection)
        {
            case Direction.right:
                vehicle.DodgeLeft();
                currentDirection = Direction.left;
                break;
            case Direction.left:
                vehicle.DodgeRight();
                currentDirection = Direction.right;
                break;
        }
        DelayManager.LocalCallTo(ChangeDirection, Random.Range(minDodgeDelay, maxDodgeDelay),this);
    }

    void ThrowProjectile()
    {
        Vector3 spawnPosition = vehicle.transform.position + vehicle.transform.right;
        Instantiate(projectile, spawnPosition, projectile.transform.rotation);
        DelayManager.LocalCallTo(ThrowProjectile, Random.Range(minShootDelay, maxShootDelay), this);
    }
}
