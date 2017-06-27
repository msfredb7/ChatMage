using FullSerializer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Moto : Car
{
    //NE PAS MODIFIER IN-GAME
    public float turnAcceleration = 5;
    public float turnSpeed = 185;
    public float moveSpeed = 1;
    public float carWeight = 1;

    [fsIgnore]
    float horizontal = 0;
    [fsIgnore]
    float lastHorizontal = 0;

    public override void OnInputUpdate(float horizontalInput)
    {
        if (horizontalInput != 0)
        {
            if ((horizontalInput < 0 && lastHorizontal > 0) || (horizontalInput > 0 && lastHorizontal < 0))
                lastHorizontal = 0;

            horizontal = Mathf.MoveTowards(lastHorizontal, horizontalInput, player.vehicle.DeltaTime() * turnAcceleration);
            player.vehicle.Rotation += -horizontal * turnSpeed * player.vehicle.DeltaTime();
        }
        lastHorizontal = horizontal;
    }

    public override void OnGameReady()
    {
        player.vehicle.weight = carWeight;
        player.vehicle.MoveSpeed = moveSpeed;
    }

    public override void OnGameStarted()
    {

    }

    public override void OnUpdate()
    {

    }
}
