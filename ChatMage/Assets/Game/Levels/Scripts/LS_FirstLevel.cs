using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using FullSerializer;

public class LS_FirstLevel : LevelScript
{
    [InspectorHeader("Enemy Prefabs"), InspectorMargin(10)]
    public GourdinierVehicle spearMan;
    public TirRocheVehicle archer;

    [InspectorHeader("Dialog"), InspectorMargin(10)]
    public Dialoguing.Dialog introDialog;
    public Dialoguing.Dialog startWavesDialog;
    public Dialoguing.Dialog firstKillDialog;

    [fsIgnore, NonSerialized]
    private bool firstWaveLaunched;

    protected override void OnGameReady()
    {
        Game.instance.smashManager.smashEnabled = false;
        Game.instance.ui.smashDisplay.canBeShown = false;

        IntroEnemies();
    }

    void IntroEnemies()
    {
        Mapping mapping = Game.instance.map.mapping;

        UnitSpawn midTopSpawn = mapping.GetSpawn("midtop");
        //UnitSpawn midLeftSpawn = mapping.GetSpawn("midleft");
        UnitSpawn midRightSpawn = mapping.GetSpawn("midright");

        midTopSpawn.SpawnUnit(spearMan).AddTargetAllegiance(Allegiance.Enemy).RemoveTargetAllegiance(Allegiance.Ally);
        //midLeftSpawn.SpawnUnit(spearMan).AddTargetAllegiance(Allegiance.Enemy).RemoveTargetAllegiance(Allegiance.Ally);
        midRightSpawn.SpawnUnit(spearMan).AddTargetAllegiance(Allegiance.Enemy).RemoveTargetAllegiance(Allegiance.Ally);
    }

    protected override void OnGameStarted()
    {
        Game.instance.ui.dialogDisplay.StartDialog(introDialog, delegate()
        {
            ReceiveEvent("tuto move");
        });

        //On start la premiere wave apres 5s (normallement, c'est le tutoriel qui le fait)
        inGameEvents.AddDelayedAction(StartFirstWave, 5);
    }

    public void StartFirstWave()
    {
        Game.instance.ui.dialogDisplay.StartDialog(startWavesDialog, delegate()
        {
            TriggerWaveManually("1st wave");
        });
    }

    public void StartSecondWave()
    {
        Game.instance.ui.dialogDisplay.StartDialog(firstKillDialog, delegate()
        {
            TriggerWaveManually("2nd wave");
        });
    }

    public override void OnReceiveEvent(string message)
    {
        switch (message)
        {
            case "wave 1 complete":
                inGameEvents.AddDelayedAction(StartSecondWave, 1);
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
        if (Input.GetKeyDown(KeyCode.D))
        {
            Time.timeScale = 0;
        }
    }
}
