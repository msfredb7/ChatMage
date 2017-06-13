using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using FullSerializer;
using FullInspector;
using UnityEngine.UI;

public class LS_demoLevelScript : LevelScript
{
    [InspectorHeader("Units")]
    public HealthPacks healthPacks;
    public ShielderVehicle shielder;

    [InspectorHeader("UI")]
    public ShowObjectives objectiveUI;

    public override void OnInit(Action onComplete)
    {
        //Game.instance.SetDefaultBorders(false, 0, false, 0);
        onComplete();
    }

    protected override void OnGameReady()
    {
        //Game.instance.gameCamera.followPlayer = true;
        inGameEvent.LockPlayerOnSpawn(90);

        //On fait gagner le joueur dans 20s
        //events.AddDelayedAction(Win, 20);

        inGameEvent.ShowUI(countdownUI).onCountdownOver += Game.instance.StartGame;

        // Objective
        inGameEvent.ShowUI(objectiveUI).AddObjective("Survive 20 seconds !");
    }

    protected override void OnGameStarted()
    {
        Game.instance.gameBounds.EnableAll();
        inGameEvent.UnLockPlayer();

        //events.SpawnEntitySpreadTime(shielder, 20, Waypoint.WaypointType.enemySpawn, 10, true);
        //events.SpawnEntitySpreadTime(healthPacks, 20, Waypoint.WaypointType.enemySpawn, 5, true);
    }

    protected override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.L) && !IsOver)
        {
            Lose();
        }
        if (Input.GetKeyDown(KeyCode.W) && !IsOver)
        {
            Win();
        }
        if (Input.GetKeyDown(KeyCode.T) && !IsOver)
        {
            TriggerWave("eersa");
        }
    }

    public override void OnReceiveEvent(string message)
    {
        switch (message)
        {
            default:
                break;
        }
    }

    public override void OnWin()
    {
        inGameEvent.Outro(true, outroUI);
    }

    public override void OnLose()
    {
        inGameEvent.Outro(false, outroUI);
    }
}
