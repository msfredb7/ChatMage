using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedievalSeigeSpawn : PointUnitSpawn
{
    public bool quitAutomaticallyAfterWave = true;
    public bool reEnableSpawingAfterQuitting = true;
    public MedievalSeigeSpawnTower tower;

    protected override void Awake()
    {
        base.Awake();
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

        tower.ArriveAnimation(() => base.SpawnUnits(units, interval));
    }
    public override void SpawnUnits(List<Unit> units, float interval, Action<Unit> callback)
    {
        float duration = (units.Count - 1) * interval;
        tower.StayAliveFor(quitAutomaticallyAfterWave, duration);

        tower.ArriveAnimation(() => base.SpawnUnits(units, interval, callback));
    }
    public override void SpawnUnits(Unit[] units, float interval)
    {
        float duration = (units.Length - 1) * interval;
        tower.StayAliveFor(quitAutomaticallyAfterWave, duration);

        tower.ArriveAnimation(() => base.SpawnUnits(units, interval));
    }
    public override void SpawnUnits(Unit[] units, float interval, Action<Unit> callback)
    {
        float duration = (units.Length - 1) * interval;
        tower.StayAliveFor(quitAutomaticallyAfterWave, duration);

        tower.ArriveAnimation(() => base.SpawnUnits(units, interval, callback));
    }


    public override void SpawnUnits(List<Unit> units, List<float> intervals)
    {
        float duration = CountIntervals(units.Count -1, intervals);
        tower.StayAliveFor(quitAutomaticallyAfterWave, duration);

        tower.ArriveAnimation(() => base.SpawnUnits(units, intervals));
    }
    public override void SpawnUnits(List<Unit> units, List<float> intervals, Action<Unit> callback)
    {
        float duration = CountIntervals(units.Count - 1, intervals);
        tower.StayAliveFor(quitAutomaticallyAfterWave, duration);

        tower.ArriveAnimation(() => base.SpawnUnits(units, intervals, callback));
    }
    public override void SpawnUnits(Unit[] units, float[] intervals)
    {
        float duration = CountIntervals(units.Length - 1, intervals);
        tower.StayAliveFor(quitAutomaticallyAfterWave, duration);

        tower.ArriveAnimation(() => base.SpawnUnits(units, intervals));
    }
    public override void SpawnUnits(Unit[] units, float[] intervals, Action<Unit> callback)
    {
        float duration = CountIntervals(units.Length - 1, intervals);
        tower.StayAliveFor(quitAutomaticallyAfterWave, duration);

        tower.ArriveAnimation(() => base.SpawnUnits(units, intervals, callback));
    }

    private float CountIntervals(int unitCount, float[] intervals)
    {
        float total = 0;
        for (int i = 0; i < unitCount; i++)
        {
            total += intervals[i % intervals.Length];
        }
        return total;
    }
    private float CountIntervals(int unitCount, List<float> intervals)
    {
        float total = 0;
        for (int i = 0; i < unitCount; i++)
        {
            total += intervals[i % intervals.Count];
        }
        return total;
    }
}
