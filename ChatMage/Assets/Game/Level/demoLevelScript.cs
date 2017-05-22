using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using FullSerializer;

public class demoLevelScript : LevelScript
{
    public float enemySpawnDelay = 4f;
    public float hpSpawnDelay = 8f;

    Unit charger;
    Unit healthPacks;

    [fsIgnore]
    Action enemySpawnAction;
    [fsIgnore]
    Action hpSpawnAction;
    [fsIgnore]
    Action winAction;
    [fsIgnore]
    Coroutine winRoutine;
    [fsIgnore]
    Coroutine enemySpawnRoutine;
    [fsIgnore]
    Coroutine hpSpawnRoutine;

    //TRES IMPORTANT DE RESET NOS VARIABLE ICI
    public override void OnGameReady()
    {
        charger = ResourceLoader.LoadEnemy("Charger");
        healthPacks = ResourceLoader.LoadMiscUnit("HealthPacks");
        Game.instance.Player.playerStats.onDeath.AddListener(Win);

        enemySpawnAction = SpawnEnemy;
        hpSpawnAction = SpawnHP;
        winAction = Win;

        Vector2 bounds = Game.instance.WorldBounds;
        Vector3 startPos = new Vector3(bounds.x / 2, bounds.y / 3);
        Game.instance.Player.transform.position = startPos;
        Game.instance.Player.vehicle.canMove.Lock("intro");
        Game.instance.Player.playerStats.canTurn.Lock("intro");
        Game.instance.Player.vehicle.targetDirection = 90;


        winRoutine = DelayManager.CallTo(winAction, 20);

        DelayManager.CallTo(delegate () { Debug.Log("3"); }, 0);
        DelayManager.CallTo(delegate () { Debug.Log("2"); }, 1);
        DelayManager.CallTo(delegate () { Debug.Log("1"); }, 2);
        DelayManager.CallTo(delegate ()
       {
           Debug.Log("Go!");
           Game.instance.StartGame();
       }, 3);
    }

    public override void OnGameStarted()
    {
        Game.instance.Player.vehicle.canMove.Unlock("intro");
        Game.instance.Player.playerStats.canTurn.Unlock("intro");

        enemySpawnAction();
        DelayManager.CallTo(hpSpawnAction, hpSpawnDelay);
    }

    void SpawnHP()
    {
        if (!isOver)
        {
            Game.instance.spawner.SpawnUnitAtRandomLocation(healthPacks, Waypoint.WaypointType.enemySpawn);
            hpSpawnRoutine = DelayManager.CallTo(hpSpawnAction, hpSpawnDelay);
        }
    }

    void SpawnEnemy()
    {
        if (!isOver)
        {
            Game.instance.spawner.SpawnUnitAtRandomLocation(charger, Waypoint.WaypointType.enemySpawn);
            enemySpawnRoutine = DelayManager.CallTo(enemySpawnAction, enemySpawnDelay);
        }
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !LoadingScreen.IsInTransition)
        {
            if (!isOver)
            {
                isOver = true;
                onObjectiveComplete.Invoke();
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Game.instance.spawner.SpawnUnitAtRandomLocation(charger, Waypoint.WaypointType.enemySpawn);
        }
    }

    void Win()
    {
        if (isOver)
            return;
        isOver = true;

        DelayManager.Cancel(winRoutine);
        DelayManager.Cancel(enemySpawnRoutine);
        DelayManager.Cancel(hpSpawnRoutine);

        Game.instance.Quit();
    }
}
