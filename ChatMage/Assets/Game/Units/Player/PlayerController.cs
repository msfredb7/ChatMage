using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vehicle vehicle;
    public Transform body;

    [Header("Temporaire")]
    public Car defaultCar;

    [System.NonSerialized]
    private Car car;
    private float horizontalInput;

    void Awake()
    {
        //Game ready event
        if (!Game.instance.gameReady)
            Game.instance.onGameReady.AddListener(OnGameReady);
        else
            OnGameReady();

        //Game started event
        if (!Game.instance.gameStarted)
            Game.instance.onGameStarted.AddListener(OnGameStarted);
        else
            OnGameStarted();
    }

    void OnGameReady()
    {
        //Temporaire
        SetCar(defaultCar);

        car.OnGameReady();
    }

    void OnGameStarted()
    {
        car.OnGameStarted();
    }
    
    public void SetCar(Car car)
    {
        this.car = car;
        car.Init(this);
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
