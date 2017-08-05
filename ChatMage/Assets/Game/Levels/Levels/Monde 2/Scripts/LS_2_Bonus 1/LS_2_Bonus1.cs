using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LS_2_Bonus1 : LevelScript
{
    public RabbitCounter counterPrefab;
    public int loseOnXRabbits = 15;

    [NonSerialized]
    private int enemyCount = 0;
    [NonSerialized]
    private bool inWave;
    [NonSerialized]
    private int waveCount = 0;
    [NonSerialized]
    private RabbitCounter counterInstance;

    protected override void OnGameReady()
    {
        base.OnGameReady();

        counterInstance = inGameEvents.SpawnUnderUI(counterPrefab);
        counterInstance.SetMax(loseOnXRabbits);

        enemyCount = 0;
        inWave = true;
        waveCount = 0;
        Game.instance.onUnitDestroyed += Instance_onUnitDestroyed;
        Game.instance.onUnitSpawned += Instance_onUnitSpawned;
    }

    private void Instance_onUnitSpawned(Unit unit)
    {
        if (unit.allegiance == Allegiance.Enemy)
        {
            enemyCount++;
            if (enemyCount >= loseOnXRabbits)
                Lose();

            if (counterInstance != null)
                counterInstance.UpdateCount(enemyCount);
        }
    }

    private void Instance_onUnitDestroyed(Unit unit)
    {
        if (unit.allegiance == Allegiance.Enemy)
        {
            enemyCount--;

            if (enemyCount == 0 && inWave)
                RabbitsKilled();

            if (counterInstance != null)
                counterInstance.UpdateCount(enemyCount);
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
            case "Win":
                Win();
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
