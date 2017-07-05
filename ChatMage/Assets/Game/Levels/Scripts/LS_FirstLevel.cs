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
    public Dialoguing.Dialog dialog;

    protected override void OnGameReady()
    {
        Game.instance.smashManager.smashEnabled = false;
        Game.instance.ui.smashDisplay.canBeShown = false;


        Mapping mapping = Game.instance.map.mapping;

        UnitSpawn midTopSpawn = mapping.GetSpawn("midtop");
        UnitSpawn midLeftSpawn = mapping.GetSpawn("midleft");
        UnitSpawn midRightSpawn = mapping.GetSpawn("midright");

        GourdinierVehicle newArcher = midTopSpawn.SpawnUnit(spearMan);
        //TirRocheBrain brain = newArcher.GetComponent<TirRocheBrain>();
        //brain.tooCloseRange = 0;
        //brain.attackingMaxRange = 1.5f;
        newArcher.AddTargetAllegiance(Allegiance.Enemy).RemoveTargetAllegiance(Allegiance.Ally);

        midLeftSpawn.SpawnUnit(spearMan).AddTargetAllegiance(Allegiance.Enemy).RemoveTargetAllegiance(Allegiance.Ally);

        midRightSpawn.SpawnUnit(spearMan).AddTargetAllegiance(Allegiance.Enemy).RemoveTargetAllegiance(Allegiance.Ally);
    }

    protected override void OnGameStarted()
    {
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
            Game.instance.ui.dialogDisplay.StartDialog(dialog, null);
        }
    }
}
