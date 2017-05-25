﻿using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using FullSerializer;
using FullInspector;

public class demoLevelScript2 : LevelScript
{
    public float enemySpawnDelay = 4f;
    public float hpSpawnDelay = 8f;

    [fsIgnore]
    GameObject countdownUI;
    [fsIgnore]
    GameObject outroUI;
    [fsIgnore]
    Unit charger;
    [fsIgnore]
    Unit healthPacks;

    //TRES IMPORTANT DE RESET NOS VARIABLE ICI
    protected override void OnGameReady()
    {
        events.LockPlayer();

        WinIn(20);

        events.ShowUI(countdownUI).GetComponent<IntroCountdown>().onCountdownOver.AddListener(GameStarted);
    }

    protected override void OnGameStarted()
    {
        events.UnLockPlayer();

        events.SpawnEntitySpreadTime(charger, 20, Waypoint.WaypointType.enemySpawn, 10, true);
        events.SpawnEntitySpreadTime(healthPacks, 20, Waypoint.WaypointType.enemySpawn, 5, true);
    }

    protected override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !LoadingScreen.IsInTransition)
        {
            if (!isOver)
            {
                isOver = true;
                onObjectiveComplete.Invoke();
            }
        }
    }

    protected override void OnEnd()
    {
        if (isOver)
            return;
        isOver = true;

        events.Outro(hasWon, outroUI);
    }

    public override void OnInit(Action onComplete)
    {
        LoadQueue queue = new LoadQueue(onComplete);
        queue.AddEnemy("Charger", (x) => charger = x);
        queue.AddMiscUnit("HealthPacks", (x) => healthPacks = x);
        queue.AddUI("Countdown", (x) => countdownUI = x);
        queue.AddUI("Outro", (x) => outroUI = x);
    }

    public override void ReceiveEvent(string message)
    {
        switch (message)
        {
            default:
                Debug.LogWarning("Demo level script received an unhandled event: " + message);
                break;
        }
    }
}
