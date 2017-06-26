using FullSerializer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Jet : Car
{
    //NE PAS MODIFIER IN-GAME
    public float turnAcceleration = 10;
    public float turnSpeed = 150;
    public float moveSpeed = 7.5f;
    public float carWeight = 0.7f;

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
        player.vehicle.weight = 0.7f;
        player.vehicle.MoveSpeed = moveSpeed;
    }

    public override void OnGameStarted()
    {

    }

    public override void OnUpdate()
    {

    }
}
