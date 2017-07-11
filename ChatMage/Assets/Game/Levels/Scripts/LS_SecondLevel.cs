using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using FullSerializer;

public class LS_SecondLevel : LevelScript
{
    [InspectorHeader("Enemy Prefabs"), InspectorMargin(10)]
    public SpearmanVehicle spearMan;
    public ArcherVehicle archer;
    public ShielderVehicle romanShielder;

    [InspectorHeader("Dialog"), InspectorMargin(10)]
    public Dialoguing.Dialog whatAmISupposeToDo;
    public Dialoguing.Dialog enterArenaDialog;
    public Dialoguing.Dialog secondWaveTalk;
    public Dialoguing.Dialog thirdWaveTalk;
    public Dialoguing.Dialog whatIsThisSorcery;
    public Dialoguing.Dialog getHim;

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
        Game.instance.smashManager.smashEnabled = false;
        Game.instance.ui.smashDisplay.canBeShown = false;

        //On ecoute a la mort de la porte
        List<Unit> allUnits = Game.instance.units;
        for (int i = 0; i < allUnits.Count; i++)
        {
            if (allUnits[i] is DestructibleDoor)
            {
                allUnits[i].onDeath += OnDestructibleDoorBroken;
                break;
            }
        }
    }

    void OnDestructibleDoorBroken(Unit door)
    {
        Game.instance.gameCamera.followPlayer = true;
    }

    protected override void OnGameStarted()
    {
        Game.instance.ui.dialogDisplay.StartDialog(whatAmISupposeToDo);

        inGameEvents.AddDelayedAction(delegate ()
        {
            ReceiveEvent("smash tutorial");
        }, 1.5f);
    }

    public void StartFirstWave()
    {
        Game.instance.ui.dialogDisplay.StartDialog(enterArenaDialog, delegate ()
        {
            TriggerWaveManually("1st wave");
        });
        TaggedObject gate = map.mapping.GetTaggedObject("arena gate");
        gate.GetComponent<SidewaysFakeGate>().Close();
        gate.GetComponent<Collider2D>().enabled = true;
    }

    public void StartSecondWave()
    {
        Game.instance.ui.dialogDisplay.StartDialog(secondWaveTalk, delegate ()
        {
            TriggerWaveManually("2nd wave");
        });
    }

    public void StartFinalWave()
    {
        Game.instance.ui.dialogDisplay.StartDialog(thirdWaveTalk, delegate ()
        {
            TriggerWaveManually("final wave");
        });
    }

    public void GoBack()
    {
        Game.instance.ui.dialogDisplay.StartDialog(whatIsThisSorcery);
        TaggedObject gate = map.mapping.GetTaggedObject("arena gate");
        gate.GetComponent<SidewaysFakeGate>().Open();
        gate.GetComponent<Collider2D>().enabled = false;
        map.roadPlayer.CurrentRoad.ApplyMinMaxToCamera();
    }

    public override void OnReceiveEvent(string message)
    {
        switch (message)
        {
            case "enter arena":
                Game.instance.gameCamera.minHeight = 0;
                StartFirstWave();
                break;
            case "wave 1 complete":
                inGameEvents.AddDelayedAction(StartSecondWave, 1);
                break;
            case "wave 2 complete":
                inGameEvents.AddDelayedAction(StartFinalWave, 1);
                break;
            case "final wave complete":
                inGameEvents.AddDelayedAction(GoBack, 1);
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
