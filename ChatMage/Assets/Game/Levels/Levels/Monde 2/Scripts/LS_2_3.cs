using FullInspector;
using FullSerializer;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LS_2_3 : LevelScript
{
    [InspectorCategory("UNIQUE"), InspectorHeader("Dialog"), InspectorMargin(10)]
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
        map = Game.Instance.map;
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
                Game.Instance.ui.dialogDisplay.StartDialog(princessSaved);
                canWin = true;
                break;
            case "ending":
                if (!canWin)
                {
                    Game.Instance.gameCamera.followPlayer = true;
                    Game.Instance.gameCamera.canScrollUp = true;
                    Game.Instance.map.roadPlayer.CurrentRoad.ApplyMinMaxToCamera();
                } else
                {
                    Game.Instance.ui.dialogDisplay.StartDialog(thankYou,delegate() {
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
