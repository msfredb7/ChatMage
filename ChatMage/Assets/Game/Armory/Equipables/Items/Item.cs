﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item : Equipable
{
    public Sprite ingameIcon;
    public string description;

    [System.NonSerialized, FullSerializer.fsIgnore]
    public int originalAssetID;

    [System.NonSerialized, FullSerializer.fsIgnore]
    private Image ballImageReference;

    public virtual void Equip(int duplicateIndex)
    {
        var itemDisplay = Game.Instance.ui.newItemsDisplay;
        if (itemDisplay != null)
        {
            Vector2 spawnPos = Game.Instance.gameCamera.cam.WorldToScreenPoint(player.vehicle.Position);
            if (Game.Instance.gameReady)
            {
                ballImageReference = itemDisplay.AddItem(ingameIcon, spawnPos);
            }
            else
            {
                ballImageReference = itemDisplay.AddItem(ingameIcon, true);
            }
        }
    }
    public virtual void Unequip()
    {
        if (Game.Instance.ui.newItemsDisplay != null)
        {
            Game.Instance.ui.newItemsDisplay.RemoveItem(ballImageReference);
        }
    }
    public override void OnGameReady() { }
    public override void OnGameStarted() { }
    public override void OnUpdate() { }
}
