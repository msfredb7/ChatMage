using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Manager;

public class SM_Warudo : Smash
{
    public float duration;
    public float targetTimeScale = 0;
    private Coroutine smashCoroutine;

    public override void OnGameReady()
    {
        player.vehicle.onDestroy += OnPlayerDestroy;
    }

    void OnPlayerDestroy(Unit player)
    {
        if (smashCoroutine != null)
            OnSmashEnd();
    }

    public override void OnGameStarted()
    {

    }

    public override void OnSmash()
    {
        Debug.Log("ZA WARUDO!");
        SetTimeScale(targetTimeScale);

        smashCoroutine = DelayManager.CallTo(OnSmashEnd, duration);
    }

    void SetTimeScale(float amount)
    {
        List<Unit> units = Game.instance.units;
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i] == Game.instance.Player.vehicle)
                continue;

            units[i].TimeScale = amount;
        }
        if (amount == 1)
            Game.instance.worldTimeScale.RemoveBuff("zwrdo");
        else
            Game.instance.worldTimeScale.AddBuff("zwrdo", amount * 100 - 100, CCC.Utility.BuffType.Percent);
    }

    void OnSmashEnd()
    {
        smashCoroutine = null;
        SetTimeScale(1);
    }

    public override void OnUpdate()
    {
    }
}
