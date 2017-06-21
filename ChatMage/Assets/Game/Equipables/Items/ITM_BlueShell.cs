using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using System;

public class ITM_BlueShell : Item
{
    [InspectorHeader("A enlever")]
    public bool enable = false; // A ENLEVER

    [InspectorHeader("Linking")]
    public BlueShellScript blueShellPrefab;

    [InspectorHeader("Settings")]
    public float spawnCooldown;

    [NonSerialized, FullSerializer.fsIgnore]
    private bool shellSpawned = false;
    [NonSerialized, FullSerializer.fsIgnore]
    private float countdown;

    public override void OnGameReady()
    {
    }

    public override void OnGameStarted()
    {
        countdown = 0;
        shellSpawned = false;

        enable = false; // A ENLEVER
    }

    public override void OnUpdate()
    {
        if (!enable)  // A ENLEVER
            return;

        if (shellSpawned)
            return;

        if (countdown < 0)
            LaunchShell();
        else
            countdown -= Time.deltaTime;
    }

    void LaunchShell()
    {
        BlueShellScript blueShell = Game.instance.SpawnUnit(blueShellPrefab, player.vehicle.Position);
        shellSpawned = true;

        blueShell.onDeath += delegate (Unit unit)
        {
            shellSpawned = false;
            countdown = spawnCooldown;
        };
    }
}
