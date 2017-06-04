using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    protected override void Start()
    {
        base.Start();
        vehicle.Init();
        //ChangeDirection();
    }

    public void Update()
    {
        if (Game.instance.Player == null || vehicle.TimeScale <= 0)
            return;

        vehicle.LookAtPlayer();

        if (vehicle.CanShoot())
            vehicle.Shoot();
    }

    //void ChangeDirection()
    //{
    //    if (Game.instance.Player == null)
    //        return;

    //    switch (currentDirection)
    //    {
    //        case Direction.right:
    //            vehicle.DodgeLeft();
    //            currentDirection = Direction.left;
    //            break;
    //        case Direction.left:
    //            vehicle.DodgeRight();
    //            currentDirection = Direction.right;
    //            break;
    //    }
    //    DelayManager.LocalCallTo(ChangeDirection, UnityEngine.Random.Range(minDodgeDelay, maxDodgeDelay),this);
    //}

    protected override void UpdatePlayer()
    {
        throw new NotImplementedException();
    }

    protected override void UpdateNoPlayer()
    {
        throw new NotImplementedException();
    }
}
