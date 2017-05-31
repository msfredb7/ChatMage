using FullSerializer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LS_DemoDragRace : LevelScript
{
    public bool followPlayer = false;
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
        player.vehicle.TeleportPosition(Game.instance.map.mapping.GetRandomSpawnPoint(Waypoint.WaypointType.PlayerSpawn).transform.position);

        events.ShowUI(countdownUI).GetComponent<IntroCountdown>().onCountdownOver.AddListener(Game.instance.StartGame);
    }

    protected override void OnGameStarted()
    {
        player.playerStats.canTurn.Unlock("lvl");
        player.vehicle.canMove.Unlock("lvl");

        if (followPlayer)
            Game.instance.map.mapFollower.FollowPlayer();
    }

    protected override void OnUpdate()
    {
    }
}
