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
    public Dialoguing.Dialog ItsATrap2;
    public Dialoguing.Dialog bossDialog;

    [fsIgnore, NonSerialized]
    private Map map;

    [fsIgnore, NonSerialized]
    private bool canWin = false;

    private Unit archerArmy;
    private TaggedObject jesusWall;
    private TaggedObject grilleWall1;
    private TaggedObject grilleWall2;

    protected override void ResetData()
    {
        base.ResetData();
        canWin = false;
    }

    protected override void OnGameReady()
    {
        map = Game.instance.map;
        TaggedObject armyWall = map.mapping.GetTaggedObject("army wall");
        armyWall.gameObject.GetComponent<ArmyWallScript>().beginMarching = true;

        grilleWall1 = map.mapping.GetTaggedObject("grille 1");

        grilleWall2 = map.mapping.GetTaggedObject("grille 2");
    }

    protected override void OnGameStarted()
    {
        Game.instance.ui.dialogDisplay.StartDialog(RUN);
        jesusWall = map.mapping.GetTaggedObject("jesus wall");
        jesusWall.gameObject.SetActive(false);
    }

    public void StartFirstWave()
    {
        grilleWall1.gameObject.SetActive(true);
        Game.instance.ui.dialogDisplay.StartDialog(ItsATrap, delegate ()
        {
            TriggerWaveManually("1st wave");
        });
    }

    public void StartSecondWave()
    {
        grilleWall2.gameObject.SetActive(true);
        Game.instance.ui.dialogDisplay.StartDialog(ItsATrap2, delegate ()
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

    public void NotDeactivatedWhenOutOfCamera(Unit unit)
    {
        AutoDeactivation deac = unit.GetComponent<AutoDeactivation>();
        if(deac != null)
            deac.enabled = false;
    }

    public override void OnReceiveEvent(string message)
    {
        switch (message)
        {
            case "spawn 1":
                inGameEvents.AddDelayedAction(StartRoadAmbushOne, 0.5f);
                break;
            case "spawn 2":
                inGameEvents.AddDelayedAction(StartRoadAmbushTwo, 0.5f);
                break;
            case "first intersec":
                inGameEvents.AddDelayedAction(StartFirstWave, 0.5f);
                break;
            case "first intersec completed":
                Game.instance.gameCamera.followPlayer = true;
                Game.instance.gameCamera.canScrollUp = true;
                grilleWall1.gameObject.SetActive(false); // animation grille
                inGameEvents.AddDelayedAction(delegate () { archerArmy.ForceDie(); }, 1);
                Game.instance.map.roadPlayer.CurrentRoad.ApplyMinMaxToCamera();
                break;
            case "spawn 3":
                inGameEvents.AddDelayedAction(StartRoadAmbushThree, 0.5f);
                break;
            case "spawn 4":
                inGameEvents.AddDelayedAction(StartRoadAmbushFour, 0.5f);
                break;
            case "second intersec":
                inGameEvents.AddDelayedAction(StartSecondWave, 0.5f);
                break;
            case "second intersec completed":
                Game.instance.gameCamera.followPlayer = true;
                Game.instance.gameCamera.canScrollUp = true;
                grilleWall2.gameObject.SetActive(false); // animation grille
                inGameEvents.AddDelayedAction(delegate () { archerArmy.ForceDie(); }, 1);
                Game.instance.map.roadPlayer.CurrentRoad.ApplyMinMaxToCamera();
                canWin = true;
                break;
            case "boss battle entry":
                LinkedListNode<Unit> node = Game.instance.units.First;
                while (node != null)
                {
                    Unit val = node.Value;

                    AutoDeactivation deac = val.GetComponent<AutoDeactivation>();
                    if (deac != null)
                        deac.enabled = true;

                    node = node.Next;
                }
                break;
            case "boss battle":
                inGameEvents.AddDelayedAction(StartBossWave, 0.5f);
                canWin = true;
                break;
            case "jesus dead":
                if (canWin)
                    Win();
                break;
        }
    }

    public void FindArcherWall(Unit unit)
    {
        if (unit is ArcherArmyUnit)
            archerArmy = unit;
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
