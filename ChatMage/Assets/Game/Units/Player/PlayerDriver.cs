using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDriver : PlayerComponent
{
    [Header("Debug")]
    public Car defaultCar;

    [System.NonSerialized]
    private Car car;
    private float horizontalInput;

    public override void Init(PlayerController controller)
    {
        base.Init(controller);

        //Temporaire
        SetCar(defaultCar);
    }


    public override void OnGameReady()
    {

        car.OnGameReady();
    }

    public override void OnGameStarted()
    {
        car.OnGameStarted();
    }

    public void SetCar(Car car)
    {
        this.car = car;
        car.Init(controller);
    }

    private void Update()
    {
        car.OnUpdate();

        if (car != null)
            car.OnInputUpdate(horizontalInput);

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
