using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_Warudo : Smash
{
    public override void OnGameReady()
    {

    }

    public override void OnGameStarted()
    {

    }

    public override void OnSmash()
    {
        Debug.Log("ZA WARUDO!");
        List<Unit> units = Game.instance.units;
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i] == Game.instance.Player.vehicle)
                continue;

            units[i].TimeScale = 0;
        }
        Game.instance.defaultSpawnTimeScale.AddBuff("zwrdo", -100, CCC.Utility.BuffType.Percent);
    }

    public override void OnUpdate()
    {

    }
}
