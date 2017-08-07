using FullInspector;
using FullSerializer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LS_2_Bonus2 : LevelScript
{
    public ITM_Axe axe;

    public int woodToCut = 10;

    public BonusScoreScript scoreUI;
    public BonusScoreScript currentScoreUI;

    [fsIgnore, NonSerialized]
    private Map map;

    private WoodSpawner woodSpawn;

    protected override void ResetData()
    {
        base.ResetData();
    }

    protected override void OnGameReady()
    {
        map = Game.instance.map;
        Game.instance.Player.playerItems.items.Add(axe);
        currentScoreUI = inGameEvents.SpawnUnderUI<BonusScoreScript>(scoreUI);
        currentScoreUI.SetScore(woodToCut);
    }

    protected override void OnGameStarted()
    {
        woodSpawn = map.mapping.GetTaggedObject("woodSpawn").GetComponent<WoodSpawner>();
        woodSpawn.woodCut += delegate () {
            currentScoreUI.ModifyScore(-1);
        };
    }

    public override void OnReceiveEvent(string message)
    {
        switch (message)
        {
            default:
                break;
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
}
