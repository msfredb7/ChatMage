using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullSerializer;

public class Car_Demo : Car
{
    //NE PAS MODIFIER IN-GAME
    public float turnClutch = 0;
    public float turnAcceleration = 5;
    public float turnSpeed = 185;

    [fsIgnore]
    float horizontal = 0;
    [fsIgnore]
    float lastHorizontal = 0;

    public override void OnInputUpdate(float horizontalInput)
    {
        if (horizontalInput == 0)
            horizontal = 0;
        else
        {
            if ((horizontalInput < 0 && lastHorizontal > 0) || (horizontalInput > 0 && lastHorizontal < 0))
                lastHorizontal = 0;

            if (Mathf.Abs(horizontalInput) > turnClutch && Mathf.Abs(lastHorizontal) < turnClutch)
                lastHorizontal = horizontalInput * turnClutch;

            horizontal = Mathf.MoveTowards(lastHorizontal, horizontalInput, player.vehicle.DeltaTime() * turnAcceleration);
        }

        player.vehicle.targetDirection += -horizontal * turnSpeed * player.vehicle.DeltaTime();

        lastHorizontal = horizontal;


        player.body.localRotation = Quaternion.Euler(0, 0, player.vehicle.targetDirection);
    }

    public override void OnGameReady()
    {

    }

    public override void OnGameStarted()
    {

    }

    public override void OnUpdate()
    {

    }
}
