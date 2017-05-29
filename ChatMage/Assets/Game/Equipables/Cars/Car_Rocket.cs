using FullSerializer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Rocket : Car
{

    //NE PAS MODIFIER IN-GAME
    public float acceleration = 0;

    public override void OnInputUpdate(float horizontalInput)
    {
        if(horizontalInput == 0)
        {
            player.vehicle.canMove.Unlock("rocket");
        } else
        {
            player.vehicle.canMove.Lock("rocket");
        }
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
