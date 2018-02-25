using FullInspector;
using FullSerializer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LS_ThridLevel : LevelScript
{
    [InspectorCategory("UNIQUE"), InspectorHeader("Wave 1")]
    public int waveOne_armyWallBefore = 1;
    [InspectorCategory("UNIQUE")]
    public string waveOne_prespawnedTag = "inter 1";
    [InspectorCategory("UNIQUE")]
    public List<Unit> waveOne_sequence = new List<Unit>();
    [InspectorCategory("UNIQUE")]
    public float waveOne_interval = 0.25f;
    [InspectorCategory("UNIQUE")]
    public string waveOne_spawnTag = "inter 1";
    [NonSerialized, NotSerialized]
    bool waveOne_reached = false;


    [InspectorCategory("UNIQUE"), InspectorHeader("Wave 2")]
    public int waveTwo_armyWallBefore = 2;
    [InspectorCategory("UNIQUE")]
    public string waveTwo_prespawnedTag = "inter 2";
    [InspectorCategory("UNIQUE")]
    public List<Unit> waveTwo_sequence = new List<Unit>();
    [InspectorCategory("UNIQUE")]
    public float waveTwo_interval = 0.25f;
    [InspectorCategory("UNIQUE")]
    public string waveTwo_spawnTag = "inter 2";
    [NonSerialized, NotSerialized]
    bool waveTwo_reached = false;


    [NonSerialized, NotSerialized]
    ArmyWallScript armyWall;

    [NonSerialized, NotSerialized]
    List<Unit> enemyBuffer = null;

    protected override void ResetData()
    {
        base.ResetData();
        waveTwo_reached = false;
        waveOne_reached = false;
        armyWall = null;
        enemyBuffer = null;
    }

    protected override void OnGameReady()
    {
        Game.Instance.smashManager.smashEnabled = true;
        Game.Instance.ui.smashDisplay.canBeShown = true;


        //Get army wall + disable collision
        armyWall = Game.Instance.map.mapping.GetTaggedObject("army wall").GetComponent<ArmyWallScript>();
        armyWall.DisableCollision();

        //On l'approche du joueur 
        Game.Instance.events.AddDelayedAction(() =>
        {
            armyWall.BringCloseToPlayer();
        },
        1.8f);
    }

    protected override void OnGameStarted()
    {
        //Re-enable la collision du army wall
        armyWall.EnableCollision();
    }

    public override void OnWin()
    {
        Armory.UnlockAccessToSmash(armoryData);
    }

    public override void OnReceiveEvent(string message)
    {
        switch (message)
        {
            case "inter 1":
                Intersect1();
                break;
            case "inter 2":
                Intersect2();
                break;
            default:
                break;
        }
    }

    private void Intersect1()
    {
        Mapping mapping = Game.Instance.map.mapping;
        List<TaggedObject> preSpawned = mapping.GetTaggedObjects(waveOne_prespawnedTag);
        Intersection(preSpawned,
            waveOne_sequence,
            waveOne_spawnTag,
            waveOne_interval,
            waveOne_armyWallBefore,
            () =>
            {
                ((GameEvents.EmptyEvent)Game.Instance.map.graph.events.Find(
                    (UnityEngine.Object obj) => obj.name == "On Wave 1 Cpl")).Trigger();
            },
            mapping.GetTaggedObject("g1 marker").GetComponent<MarkerReceiver>());

        waveOne_reached = true;
    }
    private void Intersect2()
    {
        Mapping mapping = Game.Instance.map.mapping;
        List<TaggedObject> preSpawned = mapping.GetTaggedObjects(waveTwo_prespawnedTag);
        Intersection(preSpawned,
            waveTwo_sequence,
            waveTwo_spawnTag,
            waveTwo_interval,
            waveTwo_armyWallBefore,
            () =>
            {
                ((GameEvents.EmptyEvent)Game.Instance.map.graph.events.Find(
                    (UnityEngine.Object obj) => obj.name == "On Wave 2 Cpl")).Trigger();
            },
            mapping.GetTaggedObject("g2 marker").GetComponent<MarkerReceiver>());

        waveTwo_reached = true;
    }

    private void Intersection(List<TaggedObject> preSpawned, List<Unit> wave, string spawnTag, float interval, int armyWallBefore, Action onComplete, MarkerReceiver markersTarget)
    {
        MarkerSpawner marker = Game.Instance.map.markerSpawner;
        int preSpawnedCount = preSpawned.Count;    // Les unit�es pre-spawned
        int plannedWaveCount = wave.Count;   // Les unit�es qui vont arriver d'en bas, par la wave
        int ambushCount = 0;        // Les unit�es qui ont spawn sur les cot� qui sont encore active


        float minY = Game.Instance.gameCamera.Bottom - 0.75f;
        float minX = (GameCamera.DEFAULT_SCREEN_WIDTH / -2) + 1;
        float xLength = minX.Abs() * 2;

        //Compter le nombre de units inactive dans le buffer
        if (enemyBuffer != null)
            for (int i = 0; i < enemyBuffer.Count; i++)
            {
                if (enemyBuffer[i] == null || enemyBuffer[i].IsDead)
                    continue;

                enemyBuffer[i].OnDeath -= RemoveFromBuffer;

                if (enemyBuffer[i].gameObject.activeSelf)
                {
                    //L'enemie est actif, il est donc proche du joueur.
                    // + On le raproche du joueur

                    float delta = minY - enemyBuffer[i].Position.y;
                    if (delta > 0)
                    {
                        float x = ((7.5f * ambushCount) % xLength) + minX;
                        float y = enemyBuffer[i].Position.y + delta * 0.75f;
                        enemyBuffer[i].TeleportPosition(new Vector2(x, y));
                    }
                    ambushCount++;
                }
                else
                {
                    //L'ennemie est inactif, on le tue.
                    enemyBuffer[i].ForceDie();
                }
            }

        //Le nombre de unit qu'on va spawn dans la wave sous l'�cran
        int actualWaveCount = (plannedWaveCount - ambushCount).Raised(0);

        //Le vrai nombre d'ennemie qui vont �tre pr�sent
        int totalActiveEnemies = preSpawnedCount + actualWaveCount + ambushCount;


        //Kill progress tracker
        UnitKillsProgress progressTracker = new UnitKillsProgress(totalActiveEnemies);
        //Add callbacks
        progressTracker.AddCallback(armyWall.BringCloseToPlayer, totalActiveEnemies - armyWallBefore);
        progressTracker.AddCallback(onComplete, 1f);

        //Register ambush units
        if (enemyBuffer != null)
            for (int i = 0; i < enemyBuffer.Count; i++)
            {
                if (enemyBuffer[i] != null && !enemyBuffer[i].IsDead && enemyBuffer[i].gameObject.activeSelf)
                {
                    progressTracker.RegisterUnit(enemyBuffer[i]);
                    marker.Mark(enemyBuffer[i], markersTarget);
                }
            }

        //Register les  unit pre-spawned
        for (int i = 0; i < preSpawned.Count; i++)
        {
            Unit unit = preSpawned[i].GetComponent<Unit>();
            if (unit != null)
            {
                unit.GetComponent<AI.EnemyBrainV2>().enabled = true;
                progressTracker.RegisterUnit(unit);
                marker.Mark(unit, markersTarget);
            }
        }


        //On spawn la wave sous l'ecran
        if (actualWaveCount > 0)
        {
            List<Unit> newWaveList = new List<Unit>(wave);
            newWaveList.RemoveRange(0, ambushCount.Capped(newWaveList.Count));
            Game.Instance.map.mapping.GetSpawn(spawnTag)
                .SpawnUnits(newWaveList, interval, (Unit u) =>
                {
                    progressTracker.RegisterUnit(u);
                    marker.Mark(u, markersTarget);
                });
        }


        //Clear enemyBuffer
        enemyBuffer = null;
    }

    public void AddToEnemyBuffer1(Unit unit)
    {
        if (waveOne_reached)
        {
            unit.ForceDie();
        }
        else
        {
            AddToEnemyBuffer(unit);
        }
    }

    public void AddToEnemyBuffer2(Unit unit)
    {
        if (waveTwo_reached)
        {
            unit.ForceDie();
        }
        else
        {
            AddToEnemyBuffer(unit);
        }
    }
    public void AddToEnemyBuffer(Unit unit)
    {
        if (enemyBuffer == null)
            enemyBuffer = new List<Unit>();
        enemyBuffer.Add(unit);
        unit.OnDeath += RemoveFromBuffer;
    }

    private void RemoveFromBuffer(Unit unit)
    {
        if (enemyBuffer != null)
            enemyBuffer.Remove(unit);
    }

    //private void BringEnemiesToPlayer()
    //{
    //    float minY = Game.instance.gameCamera.Bottom - 1;
    //    float minX = (GameCamera.DEFAULT_SCREEN_WIDTH / -2) + 1;
    //    float xLength = minX.Abs() * 2;
    //    float i = 0;

    //    foreach (Unit enemy in enemyBuffer)
    //    {
    //        float delta = minY - enemy.Position.y;
    //        if (delta > 0)
    //        {
    //            float x = ((7.5f * i) % xLength) + minX;
    //            float y = enemy.Position.y + delta * 0.65f;
    //            enemy.TeleportPosition(new Vector2(x, y));
    //            i++;
    //        }
    //    }
    //}
}
