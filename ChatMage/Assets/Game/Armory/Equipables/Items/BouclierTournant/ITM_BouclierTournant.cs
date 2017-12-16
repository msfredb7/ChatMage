using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_BouclierTournant : Item
{

    public GameObject shieldPrefab;

    public override void Equip(int duplicateIndex)
    {
        throw new NotImplementedException();
    }

    public override void OnGameReady()
    {
        Instantiate(shieldPrefab, player.transform);
    }

    public override void OnGameStarted()
    {
    }

    public override void OnUpdate()
    {
    }

    public override void Unequip()
    {
        throw new NotImplementedException();
    }
}
