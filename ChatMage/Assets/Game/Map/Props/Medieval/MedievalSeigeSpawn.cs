using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedievalSeigeSpawn : PointUnitSpawn
{
    public bool quitAutomaticallyAfterWave = true;
    public bool reEnableSpawingAfterQuitting = true;
    public MedievalSeigeSpawnTower tower;

    void Awake()
    {
        onCancelSpawning += MedievalSeigeSpawn_onCancelSpawning;
        tower.originalSpawn = this;
        tower.gameObject.SetActive(false);
    }

    private void MedievalSeigeSpawn_onCancelSpawning()
    {
        if (reEnableSpawingAfterQuitting)
            EnableSpawning();
    }

    public override void SpawnUnits(List<Unit> units, float interval)
    {
        float duration = (units.Count - 1) * interval;
        tower.StayAliveFor(quitAutomaticallyAfterWave, duration);

        tower.ArriveAnimation(delegate ()
        {
            for (int i = 0; i < units.Count; i++)
            {
                float delay = i * interval;
                SpawnUnit(units[i], delay);
            }
        });
    }
    public override void SpawnUnits(List<Unit> units, float interval, Action<Unit> callback)
    {
        float duration = (units.Count - 1) * interval;
        tower.StayAliveFor(quitAutomaticallyAfterWave, duration);

        tower.ArriveAnimation(delegate ()
        {
            for (int i = 0; i < units.Count; i++)
            {
                float delay = i * interval;
                SpawnUnit(units[i], delay, callback);
            }
        });
    }
    public override void SpawnUnits(Unit[] units, float interval)
    {
        float duration = (units.Length - 1) * interval;
        tower.StayAliveFor(quitAutomaticallyAfterWave, duration);

        tower.ArriveAnimation(delegate ()
        {
            for (int i = 0; i < units.Length; i++)
            {
                float delay = i * interval;
                SpawnUnit(units[i], delay);
            }
        });
    }
    public override void SpawnUnits(Unit[] units, float interval, Action<Unit> callback)
    {
        float duration = (units.Length - 1) * interval;
        tower.StayAliveFor(quitAutomaticallyAfterWave, duration);

        tower.ArriveAnimation(delegate ()
        {
            for (int i = 0; i < units.Length; i++)
            {
                float delay = i * interval;
                SpawnUnit(units[i], delay, callback);
            }
        });
    }
}
