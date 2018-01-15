﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPack : Pack
{
    [System.NonSerialized]
    public Item itemReference;

    public void SetItem(Item itemReference)
    {
        this.itemReference = itemReference;
    }

    protected override void OnPickUp()
    {
        PlayerController player = Game.Instance != null ? Game.Instance.Player : null;

        if (itemReference != null && player != null)
            player.playerItems.Equip(itemReference);
    }
}
