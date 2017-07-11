using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using FullSerializer;

public class LS_SecondLevel : LevelScript
{
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
        // Gates
        TaggedObject gate1 = map.mapping.GetTaggedObject("arena gate");
        gate1.GetComponent<SidewaysFakeGate>().Close();
        gate1.GetComponent<Collider2D>().enabled = true;
        TaggedObject gate2 = map.mapping.GetTaggedObject("fake gate top");
        gate2.GetComponent<Collider2D>().enabled = true;
    }

    public void StartSecondWave()
    {
        Game.instance.ui.dialogDisplay.StartDialog(secondWaveTalk, delegate ()
        {
            TriggerWaveManually("2nd wave");
        });
    }

    public void StartThirdWave()
    {
        Game.instance.ui.dialogDisplay.StartDialog(thirdWaveTalk, delegate ()
        {
            Game.instance.smashManager.smashEnabled = true;
            Game.instance.smashManager.enabled = true;
            Game.instance.ui.smashDisplay.canBeShown = true;
            Game.instance.smashManager.DecreaseCooldown(100);

            inGameEvents.AddDelayedAction(delegate ()
            {
                TriggerWaveManually("3rd wave");
            }, 7.5f);
        });
    }

    public void StartFourthWave()
    {
        Game.instance.ui.dialogDisplay.StartDialog(whatIsThisSorcery, delegate ()
        {
            TriggerWaveManually("4th wave");
        });
    }

    public void StartFinalWave()
    {
        Game.instance.ui.dialogDisplay.StartDialog(getHim, delegate ()
        {
            TriggerWaveManually("final wave");
            inGameEvents.AddDelayedAction(delegate ()
            {
                Game.instance.gameCamera.followPlayer = true;
                TaggedObject gate1 = map.mapping.GetTaggedObject("arena gate top");
                gate1.GetComponent<SidewaysFakeGate>().Open();
                gate1.GetComponent<Collider2D>().enabled = true;
                TaggedObject gate2 = map.mapping.GetTaggedObject("fake gate top");
                gate2.GetComponent<Collider2D>().enabled = false;
                gate2.GetComponent<SpriteRenderer>().enabled = false;
                
            }, 3);
        });
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
                inGameEvents.AddDelayedAction(StartThirdWave, 1);
                break;
            case "wave 3 complete":
                inGameEvents.AddDelayedAction(StartFourthWave, 1);
                break;
            case "wave 4 complete":
                inGameEvents.AddDelayedAction(StartFinalWave, 1);
                break;
            case "last impossible wave":
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
