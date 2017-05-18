using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public Vehicle vehicle;
    public Transform body;
    public PlayerSmash playerSmash;
    public PlayerDriver playerDriver;

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

    /// <summary>
    /// TODO: Prendre un loadout kit en parametre, ou qqchose du genre
    /// </summary>
    public void Init()
    {
        playerSmash.Init(this);
        playerDriver.Init(this);
    }

    void OnGameReady()
    {
        playerSmash.OnGameReady();
        playerDriver.OnGameReady();
    }

    void OnGameStarted()
    {
        playerSmash.OnGameStarted();
        playerDriver.OnGameStarted();
    }
}
