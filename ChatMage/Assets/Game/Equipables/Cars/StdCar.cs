using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullSerializer;
using FullInspector;

public abstract class StdCar : Car
{
    [InspectorHeader("Base Stats")]
    public float turnAcceleration = 5;
    public float turnSpeed = 185;
    public float moveSpeed = 6;
    public float weight = 0.2f;

    [fsIgnore, System.NonSerialized]
    protected float horizontal = 0;
    [fsIgnore, System.NonSerialized]
    protected float lastHorizontal = 0;

    public override void OnInputUpdate(float horizontalInput)
    {
        if (horizontalInput == 0 || !player.playerStats.receivesTurnInput)
            horizontal = 0;
        else
        {
            if ((horizontalInput < 0 && lastHorizontal > 0) || (horizontalInput > 0 && lastHorizontal < 0))
                lastHorizontal = 0;

            horizontal = Mathf.MoveTowards(lastHorizontal, horizontalInput, player.vehicle.DeltaTime() * turnAcceleration);
        }

        player.vehicle.Rotation += -horizontal * turnSpeed * player.vehicle.DeltaTime();

        lastHorizontal = horizontal;
    }

    public override void OnGameReady()
    {
        Vehicle veh = player.vehicle;
        veh.MoveSpeed = moveSpeed;
        veh.weight = weight;
    }

    public override void OnGameStarted()
    {

    }

    public override void OnUpdate()
    {

    }
}
