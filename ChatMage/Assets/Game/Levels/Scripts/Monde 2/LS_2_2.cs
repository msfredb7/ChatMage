using FullInspector;
using FullSerializer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LS_2_2 : LevelScript
{
    [InspectorHeader("Dialog"), InspectorMargin(10)]
    public Dialoguing.Dialog defendTheWall;

    [fsIgnore, NonSerialized]
    private Map map;

    [fsIgnore, NonSerialized]
    private bool canWin = false;

    private int topWaveNumber;
    private bool topDone;
    private int botWaveNumber;
    private bool botDone;

    protected override void ResetData()
    {
        base.ResetData();
        canWin = false;
    }

    protected override void OnGameReady()
    {
        map = Game.instance.map;
        topWaveNumber = 0;
        topDone = false;
        botWaveNumber = 0;
        botDone = false;
    }

    protected override void OnGameStarted()
    {
        Game.instance.ui.dialogDisplay.StartDialog(defendTheWall, delegate() {
            TriggerWaveManually("start");
        });
    }

    public override void OnReceiveEvent(string message)
    {
        switch (message)
        {
            case "start over":
                ResetRoad();
                break;
            case "top":
                TopWave();
                break;
            case "bot":
                BotWave();
                break;
            case "top1":
                ResetRoad();
                break;
            case "top2":
                ResetRoad();
                break;
            case "top3":
                ResetRoad();
                break;
            case "top4":
                ResetRoad();
                break;
            case "bot1":
                ResetRoad();
                break;
            case "bot2":
                ResetRoad();
                break;
            case "bot3":
                ResetRoad();
                break;
            case "bot4":
                ResetRoad();
                break;
            case "win":
                if(topWaveNumber >= 4 && botWaveNumber >= 4)
                {
                    Game.instance.gameCamera.followPlayer = false;
                    Win();
                }
                break;
        }
    }

    private void TopWave()
    {
        switch (topWaveNumber)
        {
            case 0:
                if (!topDone)
                {
                    StopRoad();
                    TriggerWaveManually("top1");
                    topWaveNumber++;            // prochain trigger sera une autre wave (la prochaine planifier)
                    topDone = true;             // Quand on a fini la vague du haut, on peut pas refaire une vague du haut
                    botDone = false;            // on autorise a faire la vague du bas
                }
                break;
            case 1:
                if (!topDone)
                {
                    StopRoad();
                    TriggerWaveManually("top2");
                    topWaveNumber++;
                    topDone = true;
                    botDone = false;
                }
                break;
            case 2:
                if (!topDone)
                {
                    StopRoad();
                    TriggerWaveManually("top3");
                    topWaveNumber++;
                    topDone = true;
                    botDone = false;
                }
                break;
            case 3:
                if (!topDone)
                {
                    StopRoad();
                    TriggerWaveManually("top4");
                    topWaveNumber++;
                    topDone = true;
                    botDone = false;
                }
                break;
            default:
                break;
        }
    }

    private void BotWave()
    {
        switch (topWaveNumber)
        {
            case 0:
                if (!botDone)
                {
                    StopRoad();
                    TriggerWaveManually("bot1");
                    botWaveNumber++;
                    botDone = true;
                    topDone = false;
                }
                break;
            case 1:
                if (!botDone)
                {
                    StopRoad();
                    TriggerWaveManually("bot2");
                    botWaveNumber++;
                    botDone = true;
                    topDone = false;
                }
                break;
            case 2:
                if (!botDone)
                {
                    StopRoad();
                    TriggerWaveManually("bot3");
                    botWaveNumber++;
                    botDone = true;
                    topDone = false;
                }
                break;
            case 3:
                if (!botDone)
                {
                    StopRoad();
                    TriggerWaveManually("bot4");
                    botWaveNumber++;
                    botDone = true;
                    topDone = false;
                }
                break;
            default:
                break;
        }
    }

    private void ResetRoad()
    {
        Game.instance.gameCamera.followPlayer = true;
        Game.instance.gameCamera.canScrollUp = true;
        Game.instance.map.roadPlayer.CurrentRoad.ApplyMinMaxToCamera();
    }

    private void StopRoad()
    {
        Game.instance.gameCamera.followPlayer = false;
        Game.instance.gameCamera.canScrollUp = false;
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

