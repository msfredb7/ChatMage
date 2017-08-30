using FullInspector;
using FullSerializer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LS_ThridLevel : LevelScript
{
    [NonSerialized, NotSerialized]
    ArmyWallScript armyWall;

    protected override void OnGameReady()
    {
        //Get army wall + disable collision
        armyWall = Game.instance.map.mapping.GetTaggedObject("army wall").GetComponent<ArmyWallScript>();
        armyWall.DisableCollision();

        //On l'approche du joueur 
        Game.instance.events.AddDelayedAction(() =>
        {
            armyWall.BringCloseToPlayer();
        }, 
        1.8f);
    }

    protected override void OnGameStarted()
    {
        //Re-enable la collision du army wall
        armyWall.EnableCollision();
    }
}
