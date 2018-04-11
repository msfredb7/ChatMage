﻿using FullInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_DarkMoleSword : Item
{
    public DarkMoleSword swordPrefab;
    public SharedTables sharedSlots;

    [FullSerializer.fsIgnore]
    private DarkMoleSword sword;
    private bool equipped = false;

    public override void Equip(int duplicateIndex)
    {
        base.Equip(duplicateIndex);
        sword = swordPrefab.DuplicateGO(player.transform);
        sword.gameObject.SetActive(false);
        sword.SetController(player);

        //J'ai mis se delai pour comprenser avec le manque d'animation lorsqu'on gagne un item. 
        //C'est temporaire et ca devrais etre enlever dans le futur
        Game.Instance.events.AddDelayedAction(() =>
        {
            if (!equipped)
                return;

            sword.gameObject.SetActive(true);
            int table;
            int seat;
            sharedSlots.TakeSeat(this, out table, out seat);


            int n = Mathf.CeilToInt(seat / 2f);
            float angle = DivideAlgo(n, 60);

            sword.OpenSwordSet(table, angle * (seat.IsEvenNumber() ? 1 : -1));
        }, 0.5f);

        equipped = true;
    }

    public static int DivideAlgo(int n, int max)
    {
        if (n < 0)
            return -1;
        if (n == 0)
            return 0;
        if (n == 1)
            return max;

        n--;
        int looker = n.GetLeftmostSetBit();

        int result = 0;
        while (looker >= 0)
        {
            max /= 2;
            int bit = (n >> looker) & 1;

            if (bit == 1)
                result += max;
            else
                result -= max;

            looker--;
        }

        return result;
    }

    public override void OnGameReady()
    {

    }

    public override void OnGameStarted()
    {
    }

    public override void OnUpdate()
    {
    }

    public override void Unequip()
    {
        base.Unequip();

        equipped = false;
        sharedSlots.ReleaseSeat(this);
        sword.BreakOff(()=>
        {
            if (sword != null)
                sword.DestroyGO();
        });
    }

    protected override void ClearReferences()
    {
        base.ClearReferences();
        sharedSlots.ReleaseSeat(this);
        equipped = false;
    }

    public override void PreGameClear()
    {
        base.PreGameClear();

        var tables = sharedSlots.GetTables();
        foreach (var table in tables)
        {
            table.Clear();
        }
    }

    public override float GetWeight()
    {
        return 1;
    }
}
