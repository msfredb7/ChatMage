using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_Boosters : Item
{
    public float boostDuration;

    private float timer;
    private float baseSpeed;

    public override void Init(PlayerController player)
    {
        base.Init(player);
        player.playerStats.onUnitKilled += Boost;
        baseSpeed = player.vehicle.MoveSpeed;
    }
    public override void OnGameReady()
    {
    }

    public override void OnGameStarted()
    {
    }

    public override void OnUpdate()
    {
        if(timer > 0)
            player.vehicle.MoveSpeed = baseSpeed + timer;
        else
            player.vehicle.MoveSpeed = baseSpeed;
        timer--;
    }

    void Boost(Unit unit)
    {
        timer = boostDuration;
        //player.vehicle.Bump(new Vector2(force, force), boostDuration, BumpMode.VelocityAdd);
    }
}
