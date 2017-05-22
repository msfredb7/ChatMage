using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using FullSerializer;

public class demoLevelScript : LevelScript
{
    Unit charger;
    Unit healthPacks;

    //TRES IMPORTANT DE RESET NOS VARIABLE ICI
    public override void OnGameReady()
    {
        charger = ResourceLoader.LoadEnemy("Charger");
        healthPacks = ResourceLoader.LoadMiscUnit("HealthPacks");
        Game.instance.ApplyBoundsOnUnits(Game.instance.ScreenBounds);
        Game.instance.Player.playerStats.onDeath.AddListener(EndLevel);

        Vector2 bounds = Game.instance.WorldBounds;
        Vector3 startPos = new Vector3(bounds.x / 2, bounds.y / 3);
        Game.instance.Player.transform.position = startPos;
        Game.instance.Player.vehicle.canMove.Lock("intro");
        Game.instance.Player.vehicle.targetDirection = 90;

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

        for (int i = 0; i < 1000; i++)
        {
            if (i == 0)
                continue;
            DelayManager.CallTo(delegate ()
            {
                if (!isOver)
                    Game.instance.spawner.SpawnUnitAtRandomLocation(charger, Waypoint.WaypointType.enemySpawn);
            }, 2 * i);
            DelayManager.CallTo(delegate ()
            {
                if (!isOver)
                    Game.instance.spawner.SpawnUnitAtRandomLocation(healthPacks, Waypoint.WaypointType.enemySpawn);
            }, 4 * i);
        }
        DelayManager.CallTo(delegate ()
        {
            EndLevel();
        }, 20);
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

    void EndLevel()
    {
        if (isOver)
            return;
        isOver = true;
        Game.instance.Quit();
    }
}
