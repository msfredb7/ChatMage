using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using FullSerializer;

public class Car_Kangaroo : Car, IFixedUpdate
{

    [InspectorHeader("Speed")]
    public float topSpeed = 10;

    [InspectorMargin(12), InspectorHeader("Jump")]
    public float jumpDuration = 0.4f;
    public float jumpFrequency = 1.75f;

    [InspectorMargin(12), InspectorHeader("Jump Curve")]
    public AnimationCurve speedOverTime;

    [InspectorMargin(12), InspectorHeader("Live data"), InspectorRange(0, 1f)]
    public float speed01;

    [InspectorMargin(12), InspectorHeader("Turning")]
    public float turnSpeed = 185;
    public float turnAcceleration = 5;


    [fsIgnore, System.NonSerialized]
    private float jumpTime = 0;

    public override void OnGameReady()
    {
        player.vehicle.weight = 2;
    }

    public override void OnGameStarted()
    {

    }

    public override void OnInputUpdate(float horizontalInput)
    {
        if (jumpTime < jumpDuration)
            return;

        player.vehicle.Rotation += -horizontalInput * turnSpeed * player.vehicle.DeltaTime();
    }

    public override void OnUpdate()
    {
    }

    public void RemoteFixedUpdate()
    {
        float deltaTime = player.vehicle.FixedDeltaTime();

        speed01 = speedOverTime.Evaluate(jumpTime / jumpDuration);

        player.vehicle.MoveSpeed = speed01 * topSpeed + 0.01f;

        jumpTime += deltaTime;

        if (jumpTime > 1 / jumpFrequency)
            jumpTime = 0;
    }
}
