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
    public Dialoguing.Dialog anotherDialog;
    public Dialoguing.Dialog bossDialog;

    [fsIgnore, NonSerialized]
    private Map map;

    [fsIgnore, NonSerialized]
    private bool canWin = false;

    private TaggedObject jesusWall;

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
        jesusWall = map.mapping.GetTaggedObject("jesus wall");
        jesusWall.gameObject.SetActive(false);
    }

    public void StartFirstWave()
    {
        Game.instance.ui.dialogDisplay.StartDialog(ItsATrap, delegate ()
        {
            TriggerWaveManually("1st wave");
        });
    }

    public void StartSecondWave()
    {
        Game.instance.ui.dialogDisplay.StartDialog(anotherDialog, delegate ()
        {
            TriggerWaveManually("2nd wave");
        });
    }

    public void StartBossWave()
    {
        Game.instance.ui.dialogDisplay.StartDialog(bossDialog, delegate ()
        {
            jesusWall.gameObject.SetActive(true);
            TriggerWaveManually("jesus");
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

    public void StartRoadAmbushThree()
    {
        TriggerWaveManually("road ambush 3");
    }

    public void StartRoadAmbushFour()
    {
        TriggerWaveManually("road ambush 4");
    }

    public void ChasePlayer(Unit unit)
    {
        NotDeactivatedWhenOutOfCamera(unit);
        unit.Speed *= 2;
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
                break;
            case "spawn 3":
                inGameEvents.AddDelayedAction(StartRoadAmbushThree, 1);
                break;
            case "spawn 4":
                inGameEvents.AddDelayedAction(StartRoadAmbushFour, 1);
                break;
            case "second intersec":
                inGameEvents.AddDelayedAction(StartSecondWave, 1);
                break;
            case "second intersec completed":
                Game.instance.gameCamera.followPlayer = true;
                Game.instance.gameCamera.canScrollUp = true;
                Game.instance.map.roadPlayer.CurrentRoad.ApplyMinMaxToCamera();
                canWin = true;
                break;
            case "boss battle entry":
                List<Unit> unitsInGame = Game.instance.units;
                for (int i = 0; i < unitsInGame.Count; i++)
                    unitsInGame[i].checkDeactivation = true;
                break;
            case "boss battle":
                inGameEvents.AddDelayedAction(StartBossWave, 1);
                canWin = true;
                break;
            case "jesus dead":
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
