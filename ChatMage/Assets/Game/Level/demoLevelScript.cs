using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using FullSerializer;

public class demoLevelScript : LevelScript
{
    Unit myUnit;

    //TRES IMPORTANT DE RESET NOS VARIABLE ICI
    public override void OnGameReady()
    {
        myUnit = ResourceLoader.LoadEnemy("Enemy");
    }

    public override void OnGameStarted()
    {

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
            Game.instance.spawner.SpawnUnitAtRandomLocation(myUnit, Waypoint.WaypointType.enemySpawn);
        }
    }
}
