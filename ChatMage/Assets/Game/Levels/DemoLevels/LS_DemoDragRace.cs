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
        player.playerStats.canTurn.Lock("lvl");
        player.vehicle.canMove.Lock("lvl");
        player.vehicle.TeleportDirection(90);

        events.SetPlayerOnSpawn(90);

        Game.instance.gameCamera.followPlayer = true;

        events.ShowUI(countdownUI).GetComponent<IntroCountdown>().onCountdownOver.AddListener(Game.instance.StartGame);
    }

    protected override void OnGameStarted()
    {

        events.SpawnEntitySpreadTime(dodger, 100, Waypoint.WaypointType.enemySpawn, 35, true);

        player.playerStats.canTurn.Unlock("lvl");
        player.vehicle.canMove.Unlock("lvl");

        if (followPlayer)
            Game.instance.map.mapFollower.FollowPlayer();
    }

    protected override void OnUpdate()
    {
    }
}
