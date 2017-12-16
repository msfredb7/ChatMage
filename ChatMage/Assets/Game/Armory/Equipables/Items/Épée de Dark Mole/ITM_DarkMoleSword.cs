using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_DarkMoleSword : Item
{
    public DarkMoleSword swordPrefab;

    [FullSerializer.fsIgnore]
    private DarkMoleSword sword;

    public override void Equip(int duplicateIndex)
    {
        sword = swordPrefab.DuplicateGO(player.transform);
        sword.SetController(player);
        sword.OpenSwordSet(duplicateIndex);
    }

    public override void OnGameReady()
    {
        Instantiate(swordPrefab, player.transform);
    }

    public override void OnGameStarted()
    {
    }

    public override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.U))
            player.playerItems.Unequip(this);
    }

    public override void Unequip()
    {
        sword.CloseSwords(sword.DestroyGO);
        sword = null;
    }
}
