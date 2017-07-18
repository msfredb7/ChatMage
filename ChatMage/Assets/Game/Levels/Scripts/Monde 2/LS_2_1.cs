﻿using FullInspector;
using FullSerializer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LS_2_1 : LevelScript {

    [InspectorHeader("Dialog"), InspectorMargin(10)]
    public Dialoguing.Dialog newKingdom;
    public Dialoguing.Dialog gateBlock;
    public Dialoguing.Dialog gateCleared;

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
            TriggerWaveManually("gate");
        });
    }

    public void GateCleared()
    {
        Game.instance.ui.dialogDisplay.StartDialog(gateCleared, delegate ()
        {
            gate.GetComponent<SidewaysFakeGate>().Open();
        });
    }

    public override void OnReceiveEvent(string message)
    {
        switch (message)
        {
            case "gate":
                inGameEvents.AddDelayedAction(GateBlockage, 1);
                break;
            case "gate cleared":
                GateCleared();
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
