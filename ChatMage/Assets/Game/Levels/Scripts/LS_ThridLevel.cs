using FullInspector;
using FullSerializer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LS_ThridLevel : LevelScript
{
    [InspectorHeader("Dialog"), InspectorMargin(10)]
    public Dialoguing.Dialog RUN;
    public Dialoguing.Dialog ItsATrap;

    [fsIgnore, NonSerialized]
    private Map map;

    [fsIgnore, NonSerialized]
    private bool canWin = false;

    protected override void ResetData()
    {
        base.ResetData();
        canWin = false;
    }

    protected override void OnGameReady()
    {
        map = Game.instance.map;
    }

    protected override void OnGameStarted()
    {
        Game.instance.ui.dialogDisplay.StartDialog(RUN);
    }

    public void StartFirstWave()
    {
        Game.instance.ui.dialogDisplay.StartDialog(ItsATrap, delegate ()
        {
            TriggerWaveManually("1st wave");
        });
    }

    public void StartRoadAmbushOne()
    {
        TriggerWaveManually("road ambush 1");
    }

    public void StartRoadAmbushTwo()
    {
        TriggerWaveManually("road ambush 2");
    }

    public void NotDeactivatedWhenOutOfCamera(Unit unit)
    {
        unit.checkDeactivation = false;
    }

    public override void OnReceiveEvent(string message)
    {
        switch (message)
        {
            case "spawn 1":
                inGameEvents.AddDelayedAction(StartRoadAmbushOne, 1);
                break;
            case "spawn 2":
                inGameEvents.AddDelayedAction(StartRoadAmbushTwo, 1);
                break;
            case "first intersec":
                inGameEvents.AddDelayedAction(StartFirstWave, 1);
                break;
            case "first intersec completed":
                Game.instance.gameCamera.followPlayer = true;
                Game.instance.gameCamera.canScrollUp = true;
                Game.instance.map.roadPlayer.CurrentRoad.ApplyMinMaxToCamera();
                canWin = true;
                break;
            case "attempt win":
                if (canWin)
                    Win();
                break;
        }
    }

    protected override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Win();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Lose();
        }
    }
}
