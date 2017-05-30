using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullSerializer;
using CCC.Utility;

public class Car_RocketV2 : Car
{
    //NE PAS MODIFIER IN-GAME
    public float turnClutch = 0;
    public float turnAcceleration = 5;
    public float turnSpeed = 185;
    public float moveSpeed = 6;

    [fsIgnore]
    float horizontal = 0;
    [fsIgnore]
    float lastHorizontal = 0;

    private bool wasTurning = false;
    private float charge = 0;

    public override void OnInputUpdate(float horizontalInput)
    {
        if (horizontalInput == 0 || !player.playerStats.canTurn)
        {
            //ICI
            if (wasTurning)
            {
                // boost !
                player.vehicle.canAccelerate.Unlock("car");
                player.vehicle.Speed = player.vehicle.WorldDirection2D() * moveSpeed * Mathf.Min(charge, 4f);
                charge = 0.5f;
                player.vehicle.rb.drag = 0;
                wasTurning = false;
            }
            //A ICI

            horizontal = 0;
        }
        else
        {
            //ICI
            if (!wasTurning)
            {
                //disable acc
                player.vehicle.canAccelerate.Lock("car");
                player.vehicle.rb.drag = 1;

                wasTurning = true;
            }
            //A ICI


            if ((horizontalInput < 0 && lastHorizontal > 0) || (horizontalInput > 0 && lastHorizontal < 0))
                lastHorizontal = 0;

            if (Mathf.Abs(horizontalInput) > turnClutch && Mathf.Abs(lastHorizontal) < turnClutch)
                lastHorizontal = horizontalInput * turnClutch;

            horizontal = Mathf.MoveTowards(lastHorizontal, horizontalInput, player.vehicle.DeltaTime() * turnAcceleration);
            

            //ICI
            charge += player.vehicle.DeltaTime() * 2;
            //A ICI
        }

        player.vehicle.Rotation += -horizontal * turnSpeed * player.vehicle.DeltaTime();

        lastHorizontal = horizontal;



    }

    public override void OnGameReady()
    {
        player.vehicle.moveSpeed = moveSpeed;
    }

    public override void OnGameStarted()
    {

    }

    public override void OnUpdate()
    {

    }
}
