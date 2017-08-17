using FullInspector;
using FullSerializer;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LS_2_3 : LevelScript
{
    [InspectorHeader("Dialog"), InspectorMargin(10)]
    public Dialoguing.Dialog thankYou;
    public Dialoguing.Dialog princessSaved;

    [fsIgnore, NonSerialized]
    private Map map;

    private bool canWin;

    private TaggedObject gate;

    protected override void ResetData()
    {
        base.ResetData();
    }

    protected override void OnGameReady()
    {
        canWin = false;
        map = Game.instance.map;
        gate = map.mapping.GetTaggedObject("gate");
    }

    protected override void OnGameStarted()
    {

    }

    public override void OnReceiveEvent(string message)
    {
        switch (message)
        {
            case "saveDialogEvent":
                Game.instance.ui.dialogDisplay.StartDialog(princessSaved);
                canWin = true;
                break;
            case "ending":
                if (!canWin)
                {
                    Game.instance.gameCamera.followPlayer = true;
                    Game.instance.gameCamera.canScrollUp = true;
                    Game.instance.map.roadPlayer.CurrentRoad.ApplyMinMaxToCamera();
                } else
                {
                    Game.instance.ui.dialogDisplay.StartDialog(thankYou,delegate() {
                        gate.GetComponent<SidewaysFakeGate>().Open();
                        Win();
                    });
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
