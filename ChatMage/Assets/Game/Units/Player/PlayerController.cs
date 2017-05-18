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
    public PlayerItems playerItems;
    public PlayerStats playerStats;

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
        playerDriver.Init(this);
        playerSmash.Init(this);
        playerItems.Init(this);
        playerStats.Init(this);
    }

    void OnGameReady()
    {
        playerDriver.OnGameReady();
        playerSmash.OnGameReady();
        playerItems.OnGameReady();
        playerStats.OnGameReady();
    }

    void OnGameStarted()
    {
        playerDriver.OnGameStarted();
        playerSmash.OnGameStarted();
        playerItems.OnGameStarted();
        playerStats.OnGameStarted();
    }
}
