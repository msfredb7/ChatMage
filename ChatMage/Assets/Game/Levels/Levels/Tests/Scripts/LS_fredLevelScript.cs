using System;
using UnityEngine;

public class LS_fredLevelScript : LevelScript
{
    public float cameraSize = 4.5f;
    public override void OnReceiveEvent(string message)
    {
        Debug.Log(message);
    }

    public override void OnWin()
    {

    }

    public void MakeSmart(Unit unit)
    {
        if (unit is EnemyVehicle)
            (unit as EnemyVehicle).smartMove = true;
    }

    protected override void OnGameStarted()
    {
        Game.instance.playerBounds.DisableAll();
        Game.instance.gameCamera.cam.orthographicSize = cameraSize;
    }

    protected override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.T))
            Game.instance.map.mapping.GetSpawn("plus").CancelSpawning();
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
