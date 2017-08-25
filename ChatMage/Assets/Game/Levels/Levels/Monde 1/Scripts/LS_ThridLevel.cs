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

    private TaggedObject jesusWall;
    private TaggedObject grilleWall1;
    private TaggedObject grilleWall2;
    private TaggedObject armyWall;

    // Ambush 1
    private List<Unit> soldatsAmbush1 = new List<Unit>();
    private TaggedObject soldat1;
    private TaggedObject soldat2;
    private TaggedObject soldat3;
    private TaggedObject soldat4;
    private TaggedObject soldat5;

    // Ambush 2
    private List<Unit> soldatsAmbush2 = new List<Unit>();
    private TaggedObject soldat6;
    private TaggedObject soldat7;
    private TaggedObject soldat8;
    private TaggedObject soldat9;
    private TaggedObject soldat0;

    protected override void ResetData()
    {
        base.ResetData();
        soldatsAmbush1.Clear();
        soldatsAmbush2.Clear();
        canWin = false;
    }

    protected override void OnGameReady()
    {
        map = Game.instance.map;
        
        Game.instance.gameCamera.canScrollDown = true;

        armyWall = map.mapping.GetTaggedObject("army wall");
        armyWall.gameObject.SetActive(true);
        armyWall.gameObject.GetComponent<ArmyWallScript>().beginMarching = true;

        inGameEvents.AddDelayedAction(delegate () { armyWall.gameObject.transform.position = new Vector3(-0.13f, -27.75f, 0); }, 2);

        grilleWall1 = map.mapping.GetTaggedObject("grille 1");

        grilleWall2 = map.mapping.GetTaggedObject("grille 2");

        // Ambush 1 Enemies
        soldat1 = map.mapping.GetTaggedObject("ambush 1 soldat 1");
        soldatsAmbush1.Add(soldat1.GetComponent<Vehicle>());
        soldat2 = map.mapping.GetTaggedObject("ambush 1 soldat 2");
        soldatsAmbush1.Add(soldat2.GetComponent<Vehicle>());
        soldat3 = map.mapping.GetTaggedObject("ambush 1 soldat 3");
        soldatsAmbush1.Add(soldat3.GetComponent<Vehicle>());
        soldat4 = map.mapping.GetTaggedObject("ambush 1 soldat 4");
        soldatsAmbush1.Add(soldat4.GetComponent<Vehicle>());
        soldat5 = map.mapping.GetTaggedObject("ambush 1 soldat 5");
        soldatsAmbush1.Add(soldat5.GetComponent<Vehicle>());

        // Ambush 2 Enemies
        soldat6 = map.mapping.GetTaggedObject("ambush 2 soldat 1");
        soldatsAmbush2.Add(soldat6.GetComponent<Vehicle>());
        soldat7 = map.mapping.GetTaggedObject("ambush 2 soldat 2");
        soldatsAmbush2.Add(soldat7.GetComponent<Vehicle>());
        soldat8 = map.mapping.GetTaggedObject("ambush 2 soldat 3");
        soldatsAmbush2.Add(soldat8.GetComponent<Vehicle>());
        soldat9 = map.mapping.GetTaggedObject("ambush 2 soldat 4");
        soldatsAmbush2.Add(soldat9.GetComponent<Vehicle>());
        soldat0 = map.mapping.GetTaggedObject("ambush 2 soldat 5");
        soldatsAmbush2.Add(soldat0.GetComponent<Vehicle>());
    }

    protected override void OnGameStarted()
    {
        Game.instance.ui.dialogDisplay.StartDialog(RUN);

        jesusWall = map.mapping.GetTaggedObject("jesus wall");
        jesusWall.gameObject.SetActive(false);

        // Ambush 1
        soldat1.GetComponent<Vehicle>().onDeath += OnFirstAmbushSoldierDeath;
        soldat2.GetComponent<Vehicle>().onDeath += OnFirstAmbushSoldierDeath;
        soldat3.GetComponent<Vehicle>().onDeath += OnFirstAmbushSoldierDeath;
        soldat4.GetComponent<Vehicle>().onDeath += OnFirstAmbushSoldierDeath;
        soldat5.GetComponent<Vehicle>().onDeath += OnFirstAmbushSoldierDeath;

        // Ambush 2
        soldat6.GetComponent<Vehicle>().onDeath += OnSecondAmbushSoldierDeath;
        soldat7.GetComponent<Vehicle>().onDeath += OnSecondAmbushSoldierDeath;
        soldat8.GetComponent<Vehicle>().onDeath += OnSecondAmbushSoldierDeath;
        soldat9.GetComponent<Vehicle>().onDeath += OnSecondAmbushSoldierDeath;
        soldat0.GetComponent<Vehicle>().onDeath += OnSecondAmbushSoldierDeath;
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
                //armyWall.gameObject.GetComponent<ArmyWallScript>().ForceDie(); marche pas ?

                armyWall.GetComponent<Unit>().ForceDie();

                // inGameEvents.AddDelayedAction(StartRoadAmbushOne, 0.5f); marche pas ? bug...
                break;
            case "spawn 2":
                inGameEvents.AddDelayedAction(StartRoadAmbushTwo, 0.5f);
                break;
            case "first intersec":
                // Ambush 1
                soldat1.GetComponent<Vehicle>().enabled = true;
                soldat2.GetComponent<Vehicle>().enabled = true;
                soldat3.GetComponent<Vehicle>().enabled = true;
                soldat4.GetComponent<Vehicle>().enabled = true;
                soldat5.GetComponent<Vehicle>().enabled = true;
                inGameEvents.AddDelayedAction(StartFirstWave, 0.1f);
                break;
            case "first intersec completed":
                break;
            case "spawn 3":
                inGameEvents.AddDelayedAction(StartRoadAmbushThree, 0.1f);
                inGameEvents.AddDelayedAction(StartRoadAmbushFour, 0.5f);
                break;
            case "second intersec":
                // Ambush 2
                soldat6.GetComponent<Vehicle>().enabled = true;
                soldat7.GetComponent<Vehicle>().enabled = true;
                soldat8.GetComponent<Vehicle>().enabled = true;
                soldat9.GetComponent<Vehicle>().enabled = true;
                soldat0.GetComponent<Vehicle>().enabled = true;
                inGameEvents.AddDelayedAction(StartSecondWave, 0.1f);
                break;
            case "second intersec completed":
                break;
            case "boss battle entry":
                jesusWall.gameObject.SetActive(true);
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
                inGameEvents.AddDelayedAction(StartBossWave, 0.1f);
                canWin = true;
                break;
            case "jesus dead":
                if (canWin)
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

    private void OnFirstAmbushSoldierDeath(Unit unit)
    {
        soldatsAmbush1.Remove(unit);
        if(soldatsAmbush1.Count < 1)
        {
            ResetRoad();
            grilleWall1.gameObject.SetActive(false); // animation grille
        }
    }

    private void OnSecondAmbushSoldierDeath(Unit unit)
    {
        soldatsAmbush2.Remove(unit);
        if (soldatsAmbush2.Count < 1)
        {
            ResetRoad();
            grilleWall2.gameObject.SetActive(false); // animation grille
            canWin = true;
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

    public override void OnWin()
    {
        Armory.UnlockAccessToItems();
    }
}
