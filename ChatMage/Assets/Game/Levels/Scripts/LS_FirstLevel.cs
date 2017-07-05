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
    public EnemyVehicle spearMan;
    public EnemyVehicle archer;

    [fsIgnore, NonSerialized]
    private List<Waypoint> wps;

    [fsIgnore, NonSerialized]
    Vector2 mapCenter;

    //[fsIgnore, NonSerialized]
    //private List<TaggedObject> walls;

    protected override void OnGameReady()
    {
        Game.instance.smashManager.enabled = false;
        Game.instance.ui.smashDisplay.canBeShown = false;

        Mapping mapping = Game.instance.map.mapping;
        wps = mapping.GetWaypoints(Waypoint.WaypointType.enemySpawn);
        mapCenter = Game.instance.gameCamera.Center;
        //walls = mapping.GetTaggedObjects("wall");
    }

    protected override void OnGameStarted()
    {
        //Wave1();
        ReceiveEvent("done");

    }

    public override void OnReceiveEvent(string message)
    {
        Debug.Log("Received message: " + message);
    }

    void Wave1()
    {
        Mapping mapping = Game.instance.map.mapping;
        ReadOnlyCollection<UnitSpawn> spawns = mapping.GetSpawns("ordered");

        Unit[] leftUnits = new Unit[]
        {
            spearMan,
            spearMan,
            spearMan,
            archer,
            spearMan,
            spearMan,
            archer,
            spearMan
        };

        UnitKillsProgress callbacker = new UnitKillsProgress(leftUnits.Length);

        callbacker.AddCallback(Wave1, 1f);

        spawns[0].SpawnUnits(leftUnits, 1, 3, delegate(Unit unit)
        {
            callbacker.RegisterUnit(unit);
            MoveToCenter(unit);
        });
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

    public void MoveToCenter(Unit unit)
    {
        if (!(unit is EnemyVehicle))
            return;

        EnemyVehicle enemy = unit as EnemyVehicle;
        Vector2 deltaToCenter = (mapCenter - enemy.Position).normalized;

        //Set rotation
        enemy.TeleportDirection(Vehicle.VectorToAngle(deltaToCenter));


        EnemyBrain brain = enemy.GetComponent<EnemyBrain>();

        //Disable le cerveau
        if (brain != null)
        {
            brain.enabled = false;
        }

        //On fait marcher l'ennemi vers le centre de la map
        enemy.GotoPosition(enemy.Position + deltaToCenter * 1.5f, delegate ()
        {
            //On reactive le cerveau a la fin
            if (brain != null)
                brain.enabled = true;
        });
    }
}
