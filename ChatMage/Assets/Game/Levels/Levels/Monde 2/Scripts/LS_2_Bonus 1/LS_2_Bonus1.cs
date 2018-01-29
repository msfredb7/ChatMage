using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LS_2_Bonus1 : LevelScript
{
    public RabbitCounter counterPrefab;
    public int loseOnXRabbits = 15;

    public float firstWaveMinDuration;
    public float secondWaveMinDuration;
    public float thirdWaveMinDuration;

    [NonSerialized]
    private int enemyCount = 0;
    [NonSerialized]
    private bool inWave;
    [NonSerialized]
    private int waveCount = 0;
    [NonSerialized]
    private RabbitCounter counterInstance;
    [NonSerialized]
    private float minWaveTime;

    protected override void OnGameReady()
    {
        base.OnGameReady();

        counterInstance = inGameEvents.SpawnUnderUI(counterPrefab);
        counterInstance.SetMax(loseOnXRabbits);

        enemyCount = 0;
        waveCount = 0;
        Game.Instance.onUnitDestroyed += Instance_onUnitDestroyed;
        Game.Instance.onUnitSpawned += Instance_onUnitSpawned;
    }

    private void Instance_onUnitSpawned(Unit unit)
    {
        if (unit is SlimeVehicle)
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
        if (unit is SlimeVehicle)
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
                InWave(thirdWaveMinDuration);
                break;
            case "wave 2":
                InWave(secondWaveMinDuration);
                break;
            case "wave 1":
                InWave(firstWaveMinDuration);
                break;
            case "Win":
                Win();
                break;
        }
    }

    private void InWave(float minDuration)
    {
        waveCount++;
        inGameEvents.AddDelayedAction(() => inWave = true, minDuration);
    }

    protected override void OnUpdate()
    {
    }

    void RabbitsKilled()
    {
        inWave = false;

        Map map = Game.Instance.map;

        map.roadPlayer.CurrentRoad.ApplyMaxToCamera();

        map.ResetAIArea();

        map.mapping.GetTaggedObject(waveCount + " exit").GetComponent<SidewaysFakeGate>().Open();
    }


}
