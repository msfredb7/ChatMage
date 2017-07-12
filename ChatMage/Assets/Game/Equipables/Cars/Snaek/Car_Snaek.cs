using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

public class Car_Snaek : Car, IFixedUpdate
{
    [InspectorHeader("Consecutive Power")]
    public AnimationCurve pressPower;
    public float pressPowerDuration = 3;
    public float powerDeceleration = 2;

    [InspectorMargin(12), InspectorHeader("Acceleration")]
    public float accCeiling = 3f;

    [InspectorMargin(12), InspectorHeader("Speed")]
    [InspectorRange(0, 1)]
    public float speed01;
    public float deceleration = 0.5f;

    [InspectorMargin(12), InspectorHeader("Help")]
    public float helpWhenBelow = 0.4f;
    public float helpAcc = 1;

    [InspectorMargin(12), InspectorHeader("Real Speed")]
    public float speedCeiling = 7;


    [InspectorMargin(12), InspectorHeader("Turning")]
    public float turnSpeed = 300;

    private bool currentSide = false;
    private float lastHorizontal;
    private float currentPressPower;
    private float lastPressPower;
    private float pressTime = 0;
    private bool startOfAPress = false;

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
            bool newSide = lastHorizontal > 0 ? true : false;

            if(startOfAPress && (newSide == currentSide))
            {
                lastPressPower *= 0.5f;
            }

            if (newSide != currentSide)
            {
                lastPressPower = currentPressPower;
                pressTime = 0;
                currentSide = newSide;
            }

            lastPressPower = Mathf.Max((helpWhenBelow - speed01) * helpAcc, lastPressPower);

            currentPressPower = pressPower.Evaluate(pressTime / pressPowerDuration);
            float acc = accCeiling * lastPressPower;

            speed01 += currentPressPower * acc * deltaTime;


            startOfAPress = false;
            pressTime += deltaTime;
        }
        else
        {
            pressTime = 0;
            startOfAPress = true;
            currentPressPower -= deltaTime  / powerDeceleration;
        }
        lastPressPower -= deltaTime / powerDeceleration;

        speed01 -= deceleration * deltaTime;
        speed01 = Mathf.Clamp01(speed01);

        player.vehicle.MoveSpeed = speed01 * speedCeiling + 0.01f;
    }
}
