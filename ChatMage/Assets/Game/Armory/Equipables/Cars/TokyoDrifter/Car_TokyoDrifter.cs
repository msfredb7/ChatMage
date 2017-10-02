using FullSerializer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

public class Car_TokyoDrifter : StdCar, ISpeedBuff
{
    [InspectorMargin(12), InspectorHeader("Drifting")]
    public float acceleration;
    public float deceleration;
    public float maxAddedSpeed = 5;
    public float cooldownBeforeDeceleration = 0.5f;

    [NonSerialized, fsIgnore]
    private float addedSpeed = 0;
    [NonSerialized, fsIgnore]
    private float cooldown = 0;

    public override void OnGameReady()
    {
        base.OnGameReady();
        player.vehicle.speedBuffs.Add(this);
    }
    public override void OnUpdate()
    {
        base.OnUpdate();

        float targetAddedSpeed = maxAddedSpeed;
        float accel = acceleration;

        cooldown -= player.vehicle.DeltaTime();

        //Si le joueur tourne d'un cot� ou de l'autre. On reset le cooldown
        if (player.playerDriver.LastHorizontalInput != 0)
        {
            cooldown = cooldownBeforeDeceleration;
        }

        //Si le cooldown <= 0, on ralenti
        if (cooldown <= 0)
        {
            targetAddedSpeed = 0;
            accel = deceleration;
        }

        addedSpeed = Mathf.MoveTowards(addedSpeed, targetAddedSpeed, player.vehicle.DeltaTime() * accel);
    }

    public float GetAdditionalSpeed()
    {
        return addedSpeed;
    }
}
