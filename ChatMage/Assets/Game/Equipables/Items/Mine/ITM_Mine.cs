using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

public class ITM_Mine : Item {

    [InspectorHeader("Linking")]
    public Mine minePrefab;

    [InspectorHeader("Settings")]
    public float cooldown;


    private float countdown;

    public override void OnGameReady()
    {
        
    }

    public override void OnGameStarted()
    {
        cooldown *= Game.instance.Player.playerStats.cooldownMultiplier;
    }

    public override void OnUpdate()
    {
        if (countdown < 0)
        {
            LaunchBomb(player.vehicle.Position);
        }
        countdown -= player.vehicle.DeltaTime();
    }

    void LaunchBomb(Vector2 position)
    {
        countdown = cooldown;

        Mine mine = Game.instance.SpawnUnit(minePrefab, position);
    }
}
