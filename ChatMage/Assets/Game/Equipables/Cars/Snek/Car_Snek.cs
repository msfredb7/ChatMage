using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

public class Car_Snek : Car, IFixedUpdate
{
    [InspectorHeader("Speed")]
    public float topSpeed;

    [InspectorMargin(12), InspectorHeader("Acceleration")]
    public float pressTime = 1.15f;
    public float acceleration = 2;
    public float deceleration = 1;

    [InspectorMargin(12), InspectorHeader("Speed")]
    [InspectorRange(0, 1)]
    public float speed01;

    [InspectorMargin(12), InspectorHeader("Help")]
    public float sideResetSpeed = 1;


    [InspectorMargin(12), InspectorHeader("Turning")]
    public float turnSpeed = 300;

    private float currentSide = 0;
    private float lastHorizontal;
    private float accTimer = 0;

    public override void OnGameReady()
    {
        player.vehicle.weight = 1;
        speed01 = 0;
    }

    public override void OnGameStarted()
    {

    }

    public override void OnInputUpdate(float horizontalInput)
    {
        lastHorizontal = horizontalInput;

        player.vehicle.Rotation += -horizontalInput * turnSpeed * speed01 * player.vehicle.DeltaTime();
    }

    public override void OnUpdate()
    {

    }

    public void RemoteFixedUpdate()
    {
        float deltaTime = player.vehicle.FixedDeltaTime();

        if (lastHorizontal != 0)
        {
            float newSide = lastHorizontal;

            float power = (newSide - currentSide).Abs().Capped(1);

            if (power > 0)
            {
                accTimer = power * pressTime;
                currentSide = newSide;
            }
        }
        else
        {
            currentSide = Mathf.MoveTowards(currentSide, 0, deltaTime * sideResetSpeed);
            accTimer = 0;
        }

        accTimer -= deltaTime;

        bool accelerating = accTimer >= 0;

        if (accelerating)
            speed01 = speed01.MoveTowards(1, acceleration * deltaTime);
        else
            speed01 = speed01.MoveTowards(0, deceleration * deltaTime);

        player.vehicle.MoveSpeed = speed01 * topSpeed + 0.01f;
    }
}
