using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedievalSeigeSpawn : PointUnitSpawn
{
    public MedievalSeigeSpawnTower towerPrefab;
    [Header("PAS ENCORE IMPLEMENTE")]
    public bool infiniteSpawn = false;

    public override void SpawnUnits(List<Unit> units, float interval)
    {
        NewTower().SpawnUnits(units, interval);
    }
    public override void SpawnUnits(List<Unit> units, float interval, Action<Unit> callback)
    {
        NewTower().SpawnUnits(units, interval, callback);
    }
    public override void SpawnUnits(Unit[] units, float interval)
    {
        NewTower().SpawnUnits(units, interval);
    }
    public override void SpawnUnits(Unit[] units, float interval, Action<Unit> callback)
    {
        NewTower().SpawnUnits(units, interval, callback);
    }

    private MedievalSeigeSpawnTower NewTower()
    {
        MedievalSeigeSpawnTower tower = Instantiate(towerPrefab.gameObject, Game.instance.unitsContainer)
            .GetComponent<MedievalSeigeSpawnTower>();
        tower.originalSpawn = this;
        tower.infiniteSpawn = infiniteSpawn;
        return tower;
    }
}
