using FullInspector;
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

    [InspectorHeader("Weight Conditions")]
    public int cantHaveMoreThen = 4;

    public override float GetWeight()
    {
        float ajustedWeight = 1;
        List<Item> playerItems = Game.Instance.Player.playerItems.items;
        int amountOfBlueShellsPlayerHave = 0;
        for (int i = 0; i < playerItems.Count; i++)
        {
            if (playerItems[i].GetType() == typeof(ITM_ChainChomp))
                amountOfBlueShellsPlayerHave++;
        }
        if (amountOfBlueShellsPlayerHave >= cantHaveMoreThen)
            ajustedWeight = 0;
        if (amountOfBlueShellsPlayerHave == 0)
            ajustedWeight *= 2;
        return (base.GetWeight() * ajustedWeight);
    }
}
