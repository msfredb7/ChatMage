using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using FullSerializer;
using FullInspector;

public class demoLevelScript : LevelScript
{
    public float enemySpawnDelay = 4f;
    public float hpSpawnDelay = 8f;

    [fsIgnore]
    Unit charger;
    [fsIgnore]
    Unit healthPacks;

    //TRES IMPORTANT DE RESET NOS VARIABLE ICI
    protected override void OnGameReady()
    {
        events.LockPlayer();
        
        events.EndGameEvent(20);

        events.IntroCountdown();
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
        
        Game.instance.world.UnlockLevel(1,2);
    }

    public override void OnInit(Action onComplete)
    {
        LoadQueue queue = new LoadQueue(onComplete);
        queue.AddEnemy("Charger", (x) => charger = x);
        queue.AddMiscUnit("HealthPacks", (x) => healthPacks = x);
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
