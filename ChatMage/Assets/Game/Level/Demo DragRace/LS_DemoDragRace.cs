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
    }

    protected override void OnGameReady()
    {
        player = Game.instance.Player;
        
        player.vehicle.canMove.Lock("lvl");
        player.vehicle.TeleportDirection(90);
        player.vehicle.TeleportPosition(Game.instance.map.mapping.GetRandomSpawnPoint(Waypoint.WaypointType.PlayerSpawn).transform.position);
    }

    protected override void OnGameStarted()
    {
        player.vehicle.canMove.Unlock("lvl");
    }

    protected override void OnUpdate()
    {
    }
}
