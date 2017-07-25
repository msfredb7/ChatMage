using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public PlayerVehicle vehicle;
    /// <summary>
    /// Le transform du corps. Celui ci PEUT etre scaled up/down (dependemment des items, de la situation, etc.)
    /// </summary>
    public Transform body;
    public PlayerSmash playerSmash;
    public PlayerDriver playerDriver;
    public PlayerItems playerItems;
    public PlayerStats playerStats;
    public PlayerLocations playerLocations;
    public PlayerCarTriggers playerCarTriggers;

    void Awake()
    {
        //Game ready event
        if (!Game.instance.gameReady)
            Game.instance.onGameReady += OnGameReady;
        else
            OnGameReady();

        //Game started event
        if (!Game.instance.gameStarted)
            Game.instance.onGameStarted += OnGameStarted;
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
        playerLocations.Init(this);
        playerCarTriggers.Init(this);
        vehicle.Init(this);
    }

    void OnGameReady()
    {
        playerDriver.OnGameReady();
        playerSmash.OnGameReady();
        playerItems.OnGameReady();
        playerStats.OnGameReady();
        playerLocations.OnGameReady();
        playerCarTriggers.OnGameReady();
    }

    void OnGameStarted()
    {
        playerDriver.OnGameStarted();
        playerSmash.OnGameStarted();
        playerItems.OnGameStarted();
        playerStats.OnGameStarted();
        playerLocations.OnGameStarted();
        playerCarTriggers.OnGameStarted();
    }
}
