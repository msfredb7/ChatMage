using FullInspector;
using FullSerializer;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LS_2_3 : LevelScript
{
    [InspectorHeader("Dialog"), InspectorMargin(10)]
    public Dialoguing.Dialog thankYou;

    [fsIgnore, NonSerialized]
    private Map map;

    private bool canWin;

    protected override void ResetData()
    {
        base.ResetData();
    }

    protected override void OnGameReady()
    {
        map = Game.instance.map;
        canWin = false;
    }

    protected override void OnGameStarted()
    {

    }

    public override void OnReceiveEvent(string message)
    {
        switch (message)
        {
            case "ending":
                if(canWin)
                    Win();
                else
                {
                    Game.instance.gameCamera.followPlayer = true;
                    Game.instance.gameCamera.canScrollUp = true;
                    Game.instance.map.roadPlayer.CurrentRoad.ApplyMinMaxToCamera();
                }
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
