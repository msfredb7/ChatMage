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
        chainChompInstance = Game.instance.SpawnUnit(prefab, player.vehicle.Position);
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
        if (chainChompInstance != null)
            chainChompInstance.BreakFromPlayerAndDisappear(null);
        ReleaseSeat();
    }

    protected override void ClearReferences()
    {
        base.ClearReferences();
        sharedTables.ReleaseSeat(this);
    }
}
