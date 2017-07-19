using System;
using UnityEngine;

public class LS_fredLevelScript : LevelScript
{
    public override void OnInit()
    {

    }

    public override void OnLose()
    {
    }

    public override void OnReceiveEvent(string message)
    {

    }

    public override void OnWin()
    {

    }

    public void MakeSmart(Unit unit)
    {
        if (unit is EnemyVehicle)
            (unit as EnemyVehicle).smartMove = true;
    }

    protected override void OnGameReady()
    {

    }

    protected override void OnGameStarted()
    {

    }

    protected override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.L) && !IsOver)
        {
            Lose();
        }
        if (Input.GetKeyDown(KeyCode.W) && !IsOver)
        {
            Win();
        }
    }
}
