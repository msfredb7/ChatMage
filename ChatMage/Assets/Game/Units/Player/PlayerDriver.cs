using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDriver : PlayerComponent
{
    [System.NonSerialized]
    private Car car;
    public Car Car { get { return car; } }
    public bool enableInput = true;
    public float LastHorizontalInput
    {
        get
        {
            return lastHorizontalInput;
        }
    }

    private float lastHorizontalInput;
    private float horizontalInput;
    private bool doFixedUpdate = false;

    public override void OnGameReady()
    {
        car.OnGameReady();
    }

    public override void OnGameStarted()
    {
        car.OnGameStarted();
        doFixedUpdate = car is IFixedUpdate;
    }

    public void SetCar(Car car)
    {
        this.car = car;
        car.Init(controller);
    }

    private void FixedUpdate()
    {
        if (doFixedUpdate)
            (car as IFixedUpdate).RemoteFixedUpdate();
    }

    private void Update()
    {
        car.OnUpdate();

        if (car != null && enableInput)
            car.OnInputUpdate(horizontalInput);

        lastHorizontalInput = horizontalInput;

        horizontalInput = 0;
    }

    public void TurnLeft()
    {
        horizontalInput -= 1;
    }

    public void TurnRight()
    {
        horizontalInput += 1;
    }
}
