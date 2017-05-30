using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullSerializer;
using CCC.Utility;

public class Car_Robot : Car
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

    public override void OnInputUpdate(float horizontalInput)
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // Get movement of the finger since last frame
            Vector2 touchDeltaPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

            player.vehicle.Rotation = Vehicle.VectorToAngle(touchDeltaPosition - player.vehicle.Position);
        }
    }

    public override void OnGameReady()
    {
        player.vehicle.moveSpeed = moveSpeed;
        player.vehicle.useWeight = false;
    }

    public override void OnGameStarted()
    {

    }

    public override void OnUpdate()
    {

    }
}
