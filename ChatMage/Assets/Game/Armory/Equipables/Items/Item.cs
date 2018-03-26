using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item : Equipable, IItemWeight
{
    public Sprite ingameIcon;
    public string description;

    [System.NonSerialized, FullSerializer.fsIgnore]
    public int originalAssetID;

    [System.NonSerialized, FullSerializer.fsIgnore]
    private Image ballImageReference;

    public DataSaver armorySaver;
    private const string equipableKey = "equipable_";

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

    public bool IsAvailable()
    {
        if(armorySaver.ContainsBool(equipableKey + name)){
            return armorySaver.GetBool(equipableKey + name);
        } else
        {
            Lock();
            return IsAvailable();
        }
    }

    public void Unlocked()
    {
        armorySaver.SetBool(equipableKey + name, true);
    }

    public void Lock()
    {
        armorySaver.SetBool(equipableKey + name,false);
    }

    public virtual float GetWeight()
    {
        return 1;
    }
}
