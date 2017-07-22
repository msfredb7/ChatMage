using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using System;

public class ITM_BlueShell : Item
{
    [InspectorHeader("Linking")]
    public BlueShellVehicle blueShellPrefab;

    [InspectorHeader("Settings")]
    public float spawnCooldown;

    [NonSerialized, FullSerializer.fsIgnore]
    private bool shellSpawned = false;
    [NonSerialized, FullSerializer.fsIgnore]
    private float countdown;
    [NonSerialized, FullSerializer.fsIgnore]
    private BlueShellVehicle currentBlueShell;

    public override void OnGameReady()
    {
    }

    public override void OnGameStarted()
    {
        countdown = 0;
        shellSpawned = false;
    }

    public override void OnUpdate()
    {
        if (!Game.instance.gameStarted || shellSpawned)
            return;

        if (countdown < 0)
            LaunchShell();
        else
            countdown -= player.vehicle.DeltaTime();
    }

    void LaunchShell()
    {
        //On attend que la shell precedente sois desactiver
        if (currentBlueShell != null && currentBlueShell.gameObject.activeSelf)
            return;

        if (currentBlueShell == null)
        {
            currentBlueShell = Game.instance.SpawnUnit(blueShellPrefab, player.vehicle.Position);
            currentBlueShell.onDeath += delegate (Unit unit)
            {
                shellSpawned = false;
                countdown = Cooldown;
            };
        }

        currentBlueShell.ResetValues(player.vehicle.Position);

        shellSpawned = true;
    }

    private float Cooldown { get { return spawnCooldown * player.playerStats.cooldownMultiplier; } }
}
