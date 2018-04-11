﻿using FullInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_ChainChomp : Item
{
    public ChainChomp prefab;
    public float lengthIncreaseByDuplicate = 4;
    public SharedTables sharedTables;

    [FullSerializer.fsIgnore]
    private ChainChomp chainChompInstance;

    public override void OnGameReady()
    {
    }

    public override void OnGameStarted()
    {
    }

    public override void OnUpdate()
    {
    }

    public override void Equip(int duplicateIndex)
    {
        base.Equip(duplicateIndex);

        chainChompInstance = Game.Instance.SpawnUnit(prefab, player.vehicle.Position);
        chainChompInstance.Init(player.playerLocations.boule, player);
        chainChompInstance.Spawn();
        chainChompInstance.OnDeath += (u) => ReleaseSeat();

        int myLengthIncrement = 0;
        int mySeat;
        int tableIndex;
        sharedTables.TakeSeat(this, out tableIndex, out mySeat);
        SharedTables.Table table = sharedTables.GetClientsAtTable(tableIndex);

        for (int i = 0; i < table.seats.Count; i++)
        {
            if (table.seats[i] != null && i != mySeat)
            {
                if (i > mySeat)
                {
                    myLengthIncrement++;
                }
                else
                {
                    ((ITM_ChainChomp)table.seats[i]).IncrementLength();
                }
            }
        }

        for (int i = 0; i < myLengthIncrement; i++)
        {
            IncrementLength();
        }
    }

    public void IncrementLength()
    {
        if (chainChompInstance != null)
            chainChompInstance.IncreaseLength(lengthIncreaseByDuplicate);
    }

    private void ReleaseSeat()
    {
        sharedTables.ReleaseSeat(this);
    }

    public override void Unequip()
    {
        base.Unequip();

        if (chainChompInstance != null)
            chainChompInstance.BreakFromPlayerAndDisappear(null);
        ReleaseSeat();
    }

    protected override void ClearReferences()
    {
        base.ClearReferences();
        sharedTables.ReleaseSeat(this);
    }

    public override float GetWeight()
    {
        var playerItems = Game.Instance.Player.playerItems;
        
        // Si on a déjà 1 spinner, on ne peut pas aller plus loin qu'1 chain chomp de plus
        if (playerItems.GetCountOf<ITM_Spinner>() > 0 && playerItems.GetCountOf<ITM_ChainChomp>() > 0)
            return 0;
        else
            return 1;
    }
}
