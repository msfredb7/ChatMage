using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LS_DemoDragRace : LevelScript
{
    PlayerController player;

    public override void OnInit(Action onComplete)
    {
        onComplete();
    }

    public override void ReceiveEvent(string message)
    {
    }

    protected override void OnEnd()
    {
        if (hasWon)
            Game.instance.world.UnlockLevel(1, 3);
    }

    protected override void OnGameReady()
    {
        player = Game.instance.Player;

        player.playerStats.canTurn.Lock("lvl");
        player.vehicle.canMove.Lock("lvl");
        player.vehicle.TeleportDirection(90);
        player.vehicle.TeleportPosition(Game.instance.map.mapping.GetRandomSpawnPoint(Waypoint.WaypointType.PlayerSpawn).transform.position);

        events.IntroCountdown();
    }

    protected override void OnGameStarted()
    {
        player.playerStats.canTurn.Unlock("lvl");
        player.vehicle.canMove.Unlock("lvl");

        Game.instance.map.mapFollower.FollowPlayer();
    }

    protected override void OnUpdate()
    {
    }
}
