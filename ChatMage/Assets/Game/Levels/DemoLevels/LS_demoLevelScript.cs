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

    public override void OnInit()
    {

    }

    protected override void OnGameReady()
    {
    }

    protected override void OnGameStarted()
    {

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
            TriggerWaveManually("eersa");
        }
    }

    public override void OnReceiveEvent(string message)
    {
        switch (message)
        {
            case "wave complete":
                OnWaveComplete();
                break;
            default:
                break;
        }
    }

    private void OnWaveComplete()
    {
        Game.instance.gameCamera.followPlayer = true;
        //Game.instance.gameCamera.canScrollUp = true;
        //Game.instance.gameCamera.canScrollDown = true;
    }

    public override void OnWin()
    {
    }

    public override void OnLose()
    {
    }
}
