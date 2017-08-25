using FullInspector;
using FullSerializer;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LS_2_2 : LevelScript
{
    [InspectorHeader("Dialog"), InspectorMargin(10)]
    public Dialoguing.Dialog defendTheWall;
    
    private bool topDone;
    private bool botDone;

    private List<int> possibilities = new List<int>();

    protected override void ResetData()
    {
        base.ResetData();
    }

    protected override void OnGameReady()
    {
        topDone = false;
        botDone = false;

        possibilities.Add(1);
        possibilities.Add(2);
        possibilities.Add(3);
        possibilities.Add(4);
        possibilities.Add(5);
        possibilities.Add(6);
    }

    protected override void OnGameStarted()
    {
        Game.instance.ui.dialogDisplay.StartDialog(defendTheWall);
        ResetRoad();
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
                if (!TriggerTopSiege())
                    ResetRoad();
                break;
            case "top2":
                if (!TriggerTopSiege())
                    ResetRoad();
                break;
            case "top3":
                if (!TriggerTopSiege())
                    ResetRoad();
                break;
            case "top4":
                if (!TriggerTopSiege())
                    ResetRoad();
                break;
            case "top5":
                if (!TriggerTopSiege())
                    ResetRoad();
                break;
            case "top6":
                if (!TriggerTopSiege())
                    ResetRoad();
                break;
            case "bot1":
                if (!TriggerBotSiege())
                    ResetRoad();
                break;
            case "bot2":
                if (!TriggerBotSiege())
                    ResetRoad();
                break;
            case "bot3":
                if (!TriggerBotSiege())
                    ResetRoad();
                break;
            case "bot4":
                if (!TriggerBotSiege())
                    ResetRoad();
                break;
            case "bot5":
                if (!TriggerBotSiege())
                    ResetRoad();
                break;
            case "bot6":
                if (!TriggerBotSiege())
                    ResetRoad();
                break;
            case "win":
                if(topDone && botDone)
                {
                    Game.instance.gameCamera.followPlayer = false;
                    Win();
                }
                break;
        }
    }

    private void TopWave()
    {
        if (!topDone)
        {
            possibilities.Clear();
            possibilities.Add(1);
            possibilities.Add(2);
            possibilities.Add(3);
            possibilities.Add(4);
            possibilities.Add(5);
            possibilities.Add(6);

            TriggerTopSiege();
            topDone = true;             // Quand on a fini la vague du haut, on peut pas refaire une vague du haut
        }
    }

    private bool TriggerTopSiege()
    {
        if(possibilities.Count >= 1)
        {
            int result = UnityEngine.Random.Range(0, possibilities.Count - 1);
            
            StopRoad();
            TriggerWaveManually("top" + possibilities[result]);
            possibilities.RemoveAt(result);
            return true;
        } else
        {
            return false;
        }
    }

    private void BotWave()
    {
        if (!botDone)
        {
            possibilities.Clear();
            possibilities.Add(1);
            possibilities.Add(2);
            possibilities.Add(3);
            possibilities.Add(4);
            possibilities.Add(5);
            possibilities.Add(6);

            TriggerBotSiege();
            botDone = true;             // Quand on a fini la vague du haut, on peut pas refaire une vague du haut
        }
    }

    private bool TriggerBotSiege()
    {
        if (possibilities.Count >= 1)
        {
            int result = UnityEngine.Random.Range(0, possibilities.Count - 1);

            StopRoad();
            TriggerWaveManually("bot" + possibilities[result]);
            possibilities.RemoveAt(result);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ResetRoad()
    {
        Game.instance.gameCamera.followPlayer = true;
        Game.instance.gameCamera.canScrollUp = true;
        Game.instance.map.roadPlayer.CurrentRoad.ApplyMinMaxToCamera();
        Game.instance.cadre.Disappear();
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

