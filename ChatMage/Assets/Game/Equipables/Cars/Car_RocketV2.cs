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
    public float maxBoost = 4f;
    public float dragWhileTurning = 1;
    public float speedReductionSpam = 0.5f;

    [fsIgnore]
    float horizontal = 0;
    [fsIgnore]
    float lastHorizontal = 0;

    private bool wasTurning = false;
    private float charge = 0;

    public override void OnInputUpdate(float horizontalInput)
    {
        if (horizontalInput == 0 || !player.playerStats.receivesTurnInput)
        {
            if (wasTurning)
            {
                // boost !
                player.vehicle.wheelsOnTheGround.Unlock("car");
                player.vehicle.Speed = player.vehicle.WorldDirection2D() * moveSpeed * Mathf.Min(charge, maxBoost);
                charge = speedReductionSpam;
                player.vehicle.rb.drag = 0;
                wasTurning = false;
            }

            horizontal = 0;
        }
        else
        {
            if (!wasTurning)
            {
                //disable acc
                player.vehicle.wheelsOnTheGround.Lock("car");
                player.vehicle.rb.drag = dragWhileTurning;

                wasTurning = true;
            }


            if ((horizontalInput < 0 && lastHorizontal > 0) || (horizontalInput > 0 && lastHorizontal < 0))
                lastHorizontal = 0;

            if (Mathf.Abs(horizontalInput) > turnClutch && Mathf.Abs(lastHorizontal) < turnClutch)
                lastHorizontal = horizontalInput * turnClutch;

            horizontal = Mathf.MoveTowards(lastHorizontal, horizontalInput, player.vehicle.DeltaTime() * turnAcceleration);
            
            charge += player.vehicle.DeltaTime() * 2;
        }

        player.vehicle.Rotation += -horizontal * turnSpeed * player.vehicle.DeltaTime();

        lastHorizontal = horizontal;



    }

    public override void OnGameReady()
    {
        player.vehicle.MoveSpeed = moveSpeed;
    }

    public override void OnGameStarted()
    {

    }

    public override void OnUpdate()
    {

    }
}
