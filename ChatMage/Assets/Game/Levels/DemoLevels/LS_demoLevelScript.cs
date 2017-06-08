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
    [InspectorHeader("Settings")]
    public float enemySpawnDelay = 4f;
    public float hpSpawnDelay = 8f;

    [InspectorHeader("Units")]
    public HealthPacks healthPacks;
    public ShielderVehicle shielder;

    [InspectorHeader("UI")]
    public IntroCountdown countdownUI;
    public GameResultUI outroUI;
    public ShowObjectives objectiveUI;

    public override void OnInit(Action onComplete)
    {
        Game.instance.SetDefaultBorders(true, 0, true, 0);
        onComplete();
    }

    protected override void OnGameReady()
    {
        events.LockPlayerOnSpawn(90);

        //On fait gagner le joueur dans 20s
        events.AddDelayedAction(Win, 20);

        events.ShowUI(countdownUI).onCountdownOver += Game.instance.StartGame;

        // Objective
        events.ShowUI(objectiveUI).AddObjective("Survive 20 seconds !");
    }

    protected override void OnGameStarted()
    {
        Game.instance.gameBounds.EnableAll();
        events.UnLockPlayer();

        events.SpawnEntitySpreadTime(shielder, 20, Waypoint.WaypointType.enemySpawn, 10, true);
        events.SpawnEntitySpreadTime(healthPacks, 20, Waypoint.WaypointType.enemySpawn, 5, true);
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

    public override void OnWin()
    {
        events.Outro(true, outroUI);
    }

    public override void OnLose()
    {
        events.Outro(false, outroUI);
    }
}
