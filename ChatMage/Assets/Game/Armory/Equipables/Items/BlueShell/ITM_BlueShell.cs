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
    public float cooldown = 20;
    public float startDelay = 0.75f;

    [NonSerialized, FullSerializer.fsIgnore]
    private bool shellSpawned = false;
    [NonSerialized, FullSerializer.fsIgnore]
    private float countdown;
    [NonSerialized, FullSerializer.fsIgnore]
    private BlueShellVehicle currentBlueShell;

    public override void OnUpdate()
    {
        if (!Game.Instance.gameStarted || shellSpawned)
            return;

        if (CanLaunchShell())
            LaunchShell();
        else
            countdown -= player.vehicle.DeltaTime();
    }

    private bool CanLaunchShell()
    {
        return countdown < 0 && (currentBlueShell == null || !currentBlueShell.gameObject.activeSelf);
    }

    void LaunchShell()
    {
        if (currentBlueShell == null)
        {
            currentBlueShell = Game.Instance.SpawnUnit(blueShellPrefab, player.vehicle.Position);
            currentBlueShell.OnDeath += delegate (Unit unit)
            {
                shellSpawned = false;
                countdown = Cooldown;
            };
        }

        currentBlueShell.ResetValues(player.vehicle.Position);

        shellSpawned = true;
    }

    public override void Equip(int duplicateIndex)
    {
        base.Equip(duplicateIndex);

        countdown = startDelay;
        shellSpawned = false;
    }

    public override void Unequip()
    {
        base.Unequip();
    }

    private float Cooldown { get { return cooldown * player.playerStats.cooldownMultiplier; } }

    // WEIGHT

    [InspectorHeader("Weight Conditions")]
    public int cantHaveMoreThen = 8;

    public override float GetWeight()
    {
        float ajustedWeight = 1;
        List<Item> playerItems = Game.Instance.Player.playerItems.items;
        int amountOfBlueShellsPlayerHave = 0;
        for (int i = 0; i < playerItems.Count; i++)
        {
            if (playerItems[i].GetType() == typeof(ITM_BlueShell))
                amountOfBlueShellsPlayerHave++;
        }
        if (amountOfBlueShellsPlayerHave >= cantHaveMoreThen)
            ajustedWeight = 0;
        return (base.GetWeight() * ajustedWeight);
    }
}
