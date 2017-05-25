using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgerBrain : EnemyBrain<DodgerVehicle>
{
    public float minDodgeDelay = 1;
    public float maxDodgeDelay = 2;

    public bool changeDirection;
    public enum Direction
    {
        right = 0,
        left = 1
    }

    public Direction currentDirection;

    protected override void Start()
    {
        base.Start();
        vehicle.Init();
    }

    public void Update()
    {
        vehicle.LookAtPlayer();

        if (changeDirection)
            ChangeDirection();

        if(currentDirection == 0)
        {
            vehicle.DodgeRight();
        } else
        {
            vehicle.DodgeLeft();
        }
    }

    void ChangeDirection()
    {
        switch (currentDirection)
        {
            case Direction.right:
                currentDirection = Direction.left;
                break;
            case Direction.left:
                currentDirection = Direction.right;
                break;
        }
        changeDirection = false;
        DelayManager.CallTo(delegate ()
        {
            changeDirection = true;
        }, Random.Range(minDodgeDelay, maxDodgeDelay));

    }
}
