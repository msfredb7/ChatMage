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
    public Dialoguing.Dialog whereAmIDialog;
    public Dialoguing.Dialog lookADoorDialog;
    public Dialoguing.Dialog enterArenaDialog;
    public Dialoguing.Dialog firstKillDialog;

    [fsIgnore, NonSerialized]
    private bool firstWaveLaunched;

    private Map map;

    protected override void OnGameReady()
    {
        map = Game.instance.map;
        Game.instance.smashManager.smashEnabled = false;
        Game.instance.ui.smashDisplay.canBeShown = false;


        //On ecoute a la mort de la porte
        List<Unit> allUnits = Game.instance.units;
        for (int i = 0; i < allUnits.Count; i++)
        {
            if(allUnits[i] is DestructibleDoor)
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
        Game.instance.ui.dialogDisplay.StartDialog(whereAmIDialog);

        inGameEvents.AddDelayedAction(delegate ()
        {
            ReceiveEvent("tuto move");
        }, 1.5f);
    }

    public void StartFirstWave()
    {
        Game.instance.ui.dialogDisplay.StartDialog(enterArenaDialog, delegate()
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
            case "face door":
                Game.instance.ui.dialogDisplay.StartDialog(lookADoorDialog);
                break;
            case "enter arena":
                Game.instance.gameCamera.minHeight = 0;
                StartFirstWave();
                map.mapping.GetTaggedObject("arena gate").GetComponent<SidewaysFakeGate>().Close();
                break;
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
    }
}
