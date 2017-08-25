using FullInspector;
using FullSerializer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LS_2_1 : LevelScript {

    [InspectorHeader("Dialog"), InspectorMargin(10)]
    public Dialoguing.Dialog newKingdom;
    public Dialoguing.Dialog gateBlock;

    [fsIgnore, NonSerialized]
    private Map map;

    [fsIgnore, NonSerialized]
    private bool canWin = false;

    private TaggedObject gate;

    protected override void ResetData()
    {
        base.ResetData();
        canWin = false;
    }

    protected override void OnGameReady()
    {
        map = Game.instance.map;
        gate = map.mapping.GetTaggedObject("gate");
    }

    protected override void OnGameStarted()
    {
        Game.instance.ui.dialogDisplay.StartDialog(newKingdom);
    }

    public void GateBlockage()
    {
        Game.instance.ui.dialogDisplay.StartDialog(gateBlock, delegate ()
        {
            gate.GetComponent<SidewaysFakeGate>().Open();
            ResetRoad();
            canWin = true;
        });
    }

    public override void OnReceiveEvent(string message)
    {
        switch (message)
        {
            case "start":
                break;
            case "survival1":
                TriggerWaveManually("survival1");
                break;
            case "survival1 complete":
                ResetRoad();
                break;
            case "survival2":
                TriggerWaveManually("survival2");
                break;
            case "survival2 complete":
                ResetRoad();
                break;
            case "survival3":
                TriggerWaveManually("survival3");
                break;
            case "survival3 complete":
                ResetRoad();
                break;
            case "survival4":
                TriggerWaveManually("survival4");
                break;
            case "survival4 complete":
                ResetRoad();
                break;
            case "survival5":
                TriggerWaveManually("survival5");
                break;
            case "survival5 complete":
                ResetRoad();
                break;
            case "survival6":
                TriggerWaveManually("survival6");
                break;
            case "survival6 complete":
                ResetRoad();
                break;
            case "survival7":
                TriggerWaveManually("survival7");
                break;
            case "survival7 complete":
                ResetRoad();
                break;
            case "survival8":
                TriggerWaveManually("survival8");
                break;
            case "survival8 complete":
                ResetRoad();
                break;
            case "gate":
                GateBlockage();
                break;
            case "win":
                if(canWin)
                    Win();
                break;
        }
    }

    private void ResetRoad()
    {
        Game.instance.gameCamera.followPlayer = true;
        Game.instance.gameCamera.canScrollUp = true;
        Game.instance.map.roadPlayer.CurrentRoad.ApplyMinMaxToCamera();
        Game.instance.cadre.Disappear();
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
