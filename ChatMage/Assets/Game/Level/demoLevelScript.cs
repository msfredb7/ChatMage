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
        Game.instance.Player.GetComponent<PlayerStats>().onDeath.AddListener(EndLevel);
    }

    public override void OnGameStarted()
    {
        for(int i = 0; i < 1000; i++)
        {
            if (i == 0)
                continue;
            DelayManager.CallTo(delegate ()
            {
                if(!isOver)
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
