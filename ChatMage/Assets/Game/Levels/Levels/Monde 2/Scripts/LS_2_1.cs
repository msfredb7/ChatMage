using FullInspector;
using FullSerializer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LS_2_1 : LevelScript {

    protected override void ResetData()
    {

    }

    protected override void OnGameReady()
    {

    }

    protected override void OnGameStarted()
    {
        
    }

    public override void OnReceiveEvent(string message)
    {

    }

    private void ResetRoad()
    {
        Game.Instance.gameCamera.followPlayer = true;
        Game.Instance.gameCamera.canScrollUp = true;
        Game.Instance.map.roadPlayer.CurrentRoad.ApplyMinMaxToCamera();
        Game.Instance.cadre.Disappear();
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
