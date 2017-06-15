using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_DriftSpeed : Item
{
    public float acceleration;

    private float baseSpeed;

    public override void Init(PlayerController player)
    {
        base.Init(player);
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
        if(player.vehicle.canTurn)
        {
            player.vehicle.MoveSpeed += Time.deltaTime * acceleration;
        } else
            player.vehicle.MoveSpeed = baseSpeed;

        Debug.Log(player.vehicle.MoveSpeed);

    }
}
