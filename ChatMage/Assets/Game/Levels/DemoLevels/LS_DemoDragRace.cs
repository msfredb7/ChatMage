using FullSerializer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LS_DemoDragRace : LevelScript
{
    public bool followPlayer = false;
    public GourdinierVehicle gourdinier;
    public TirRocheVehicle tirRoche;
    public ShielderVehicle shielder;
    PlayerController player;

    [fsIgnore]
    GameObject countdownUI;
    [fsIgnore]
    GameObject outroUI;

    public override void OnInit(Action onComplete)
    {
        LoadQueue queue = new LoadQueue(onComplete);
        queue.AddUI("Countdown", (x) => countdownUI = x);
        queue.AddUI("Outro", (x) => outroUI = x);

        Game.instance.SetDefaultBorders(true, 0, true, 0);
    }

    public override void ReceiveEvent(string message)
    {
    }

    public override void OnEnd()
    {
        events.Outro(hasWon, outroUI);
    }

    protected override void OnGameReady()
    {
        player = Game.instance.Player;

        Game.instance.gameCamera.SetToHeight(
            events.LockPlayerOnSpawn(90).y);

        events.ShowUI(countdownUI).GetComponent<IntroCountdown>().onCountdownOver.AddListener(Game.instance.StartGame);

        Game.instance.gameCamera.followPlayer = followPlayer;
    }

    protected override void OnGameStarted()
    {
        Game.instance.gameBounds.EnableAll();
        //Game.instance.SpawnUnit(tirRoche, Vector2.left * 5);
        //events.SpawnEntitySpreadTime(dodger, 100, Waypoint.WaypointType.enemySpawn, 35, true);

        events.UnLockPlayer();

    }

    protected override void OnUpdate()
    {
        if(Input.GetKeyDown(KeyCode.S))
            Game.instance.SpawnUnit(tirRoche, Vector2.left * 5);
    }
}
