using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using FullSerializer;
using FullInspector;

public class LS_Level3 : LevelScript {

    public float enemySpawnDelay = 4f;
    public float hpSpawnDelay = 8f;

    [fsIgnore]
    GameObject countdownUI;
    [fsIgnore]
    GameObject outroUI;
    [fsIgnore]
    Unit dodger;
    [fsIgnore]
    int dodgerKilled;

    //TRES IMPORTANT DE RESET NOS VARIABLE ICI
    protected override void OnGameReady()
    {
        events.LockPlayer();
        dodgerKilled = 0;

        events.ShowUI(countdownUI).GetComponent<IntroCountdown>().onCountdownOver.AddListener(Game.instance.StartGame);
    }

    protected override void OnGameStarted()
    {
        events.UnLockPlayer();

        events.SpawnEntitySpreadTime(dodger, 100, Waypoint.WaypointType.enemySpawn, 35, true, AddDeathListener);
    }

    void AddDeathListener(Unit unit)
    {
        (unit as DodgerVehicle).onDestroy += DodgerKilled;
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
        if(dodgerKilled > 4)
            events.WinIn(0);
    }

    public override void OnQuit()
    {
        if (isOver)
            return;
        isOver = true;

        events.Outro(hasWon, outroUI);
    }

    public override void OnInit(Action onComplete)
    {
        LoadQueue queue = new LoadQueue(onComplete);
        queue.AddEnemy("Dodger", (x) => dodger = x);
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

    void DodgerKilled(Unit unit)
    {
        dodgerKilled++;
    }
}
