using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LS_2_Bonus1 : LevelScript
{
    [NonSerialized]
    private int enemyCount = 0;
    [NonSerialized]
    private bool inWave;
    [NonSerialized]
    private int waveCount = 0;

    protected override void OnGameReady()
    {
        base.OnGameReady();

        enemyCount = 0;
        inWave = true;
        waveCount = 0;
        Game.instance.onUnitDestroyed += Instance_onUnitDestroyed;
        Game.instance.onUnitSpawned += Instance_onUnitSpawned;
    }

    private void Instance_onUnitSpawned(Unit unit)
    {
        if (unit.allegiance == Allegiance.Enemy)
            enemyCount++;
    }

    private void Instance_onUnitDestroyed(Unit unit)
    {
        if (unit.allegiance == Allegiance.Enemy)
        {
            enemyCount--;

            if (enemyCount == 0 && inWave)
                RabbitsKilled();
        }
    }

    public override void OnReceiveEvent(string message)
    {
        switch (message)
        {
            case "wave 3":
            case "wave 2":
            case "wave 1":
                waveCount++;
                inWave = true;
                break;
        }
    }

    void RabbitsKilled()
    {
        inWave = false;

        Map map = Game.instance.map;

        map.roadPlayer.CurrentRoad.ApplyMaxToCamera();

        map.mapping.GetTaggedObject(waveCount + " exit").GetComponent<SidewaysFakeGate>().Open();
    }


}
