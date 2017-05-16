using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoDriver : PlayerDriver
{
    const float turnClutch = 0;
    const float turnAcceleration = 5;
    const float turnSpeed = 185;
    float horizontal = 0;
    float lastHorizontal = 0;
    float rotation;

    public DemoDriver(PlayerController player) : base(player)
    {
        rotation = player.vehicle.targetDirection;
    }

    public override void Update(float horizontalInput)
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
        
        rotation += -horizontal * turnSpeed * player.vehicle.DeltaTime();

        lastHorizontal = horizontal;


        player.body.localRotation = Quaternion.Euler(0, 0, rotation);
        player.vehicle.targetDirection = rotation;
    }
}
