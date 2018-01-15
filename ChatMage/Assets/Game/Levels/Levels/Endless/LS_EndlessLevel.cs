using LevelScripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LS_EndlessLevel : LevelScript
{
    // Gates
    SidewaysFakeGate topgate;
    SidewaysFakeGate botgate;

    // Spawn
    string spawnTag = "random";
    public List<Unit> possibleUnits;
    public float spawnInterval = 0.25f;

    protected override void OnGameReady()
    {
        Game.Instance.smashManager.smashEnabled = true;
        Game.Instance.ui.smashDisplay.canBeShown = true;

        // Obtenir les références nécessaire
        topgate = Game.Instance.map.mapping.GetTaggedObject("topgate").GetComponent<SidewaysFakeGate>();
        botgate = Game.Instance.map.mapping.GetTaggedObject("botgate").GetComponent<SidewaysFakeGate>();

    }

    protected override void OnGameStarted()
    {
        botgate.Close();
        SpawnWave();
    }

    public override void OnReceiveEvent(string message)
    {
        switch (message)
        {
            default:
                break;
        }
    }

    private void SpawnWave()
    {
        UnitWaveV2 wave = new UnitWaveV2();
        wave.infiniteRepeat = true;
        wave.pauseBetweenRepeat = 0;
        wave.spawnInterval = 1;
        wave.preLaunchDialog = null;

        // What ?
        wave.what = new WaveWhat();
        wave.what.spawnSequence = new UnitPack[1];
        wave.what.spawnSequence[0] = new UnitPack();
        wave.what.spawnSequence[0].unit = possibleUnits[0];
        wave.what.spawnSequence[0].quantity = 1;

        // Where ?
        wave.where = new WaveWhereV2();
        wave.where.spawnTag = spawnTag;
        wave.where.filterType = WaveWhereV2.FilterType.None;
        wave.where.selectType = WaveWhereV2.SelectType.ByIndex;
        wave.where.index = 0;

        // When ?
        wave.when = new WaveWhen();
        wave.when.type = WaveWhen.Type.OnManualTrigger;
        wave.when.name = "wave";
        wave.when.onlyTriggerOnce = false;
        
        //public event SimpleEvent onLaunched;
        //public event SimpleEvent onComplete;
        //public WaveWhat what;
        //public bool infiniteRepeat;
        //public float pauseBetweenRepeat;
        //public WaveWhereV2 where;
        //public WaveWhen when;
        //public float spawnInterval;
        //public Dialoguing.Dialog preLaunchDialog;
        ManuallyAddWave(wave);
        TriggerWaveManually("wave");
    }
}
