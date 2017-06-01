using FullSerializer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LS_DemoDragRace : LevelScript
{
    public bool followPlayer = false;
    public DodgerVehicle dodger;
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

        events.LockPlayerOnSpawn(90);

        events.ShowUI(countdownUI).GetComponent<IntroCountdown>().onCountdownOver.AddListener(Game.instance.StartGame);
    }

    protected override void OnGameStarted()
    {
        //events.SpawnEntitySpreadTime(dodger, 100, Waypoint.WaypointType.enemySpawn, 35, true);

        events.UnLockPlayer();

        Game.instance.gameCamera.followPlayer = followPlayer;
    }

    protected override void OnUpdate()
    {
    }
}
