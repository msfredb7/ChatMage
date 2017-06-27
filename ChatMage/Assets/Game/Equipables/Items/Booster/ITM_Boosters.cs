using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using FullSerializer;

public class ITM_Boosters : Item
{
    public float stackDuration;
    public int maxBoostStacks = 3;
    public float moveSpeedMultiplierPerStack = 1.2f;

    [fsIgnore, NonSerialized]
    private float remainingBoostTime;
    [fsIgnore, NonSerialized]
    private int boostStacks = 0;
    [fsIgnore, NonSerialized]
    private float baseSpeed;

    public override void Init(PlayerController player)
    {
        base.Init(player);
        player.playerStats.onUnitKilled += Boost;
    }
    public override void OnGameReady()
    {
        baseSpeed = player.vehicle.MoveSpeed;
    }

    public override void OnGameStarted()
    {
    }

    public override void OnUpdate()
    {
        if (remainingBoostTime <= 0 && boostStacks > 0)
            LoseStacks();

        remainingBoostTime -= player.vehicle.DeltaTime();
    }

    void LoseStacks()
    {
        player.vehicle.MoveSpeed = baseSpeed;
        boostStacks = 0;
    }

    void Boost(Unit unit)
    {
        //Increase speed
        if (boostStacks < maxBoostStacks)
        {
            boostStacks++;
            player.vehicle.MoveSpeed *= moveSpeedMultiplierPerStack;
        }

        //Refresh timer
        remainingBoostTime = stackDuration;
    }
}
