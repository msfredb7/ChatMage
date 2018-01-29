using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItems : PlayerComponent
{
    public List<Item> items = new List<Item>();
    public event SimpleEvent OnItemListChange;

    public int ItemCount { get { return items.Count; } }

    public override void Init(PlayerController controller)
    {
        base.Init(controller);
    }


    public void Equip(List<Item> items)
    {
        foreach (Item item in items)
        {
            Equip(item);
        }
    }
    public void Equip(Item itemAssetReference)
    {
        Item newItem = Instantiate(itemAssetReference);
        newItem.originalAssetID = itemAssetReference.GetInstanceID();
        newItem.Init(controller);

        //Duplicate index = le nombre d'item identique existant deja
        int duplicateIndex = GetDuplicateCount_Asset(itemAssetReference);
        items.Add(newItem);
        newItem.Equip(duplicateIndex);

        if (OnItemListChange != null)
            OnItemListChange();
    }

    public int GetDuplicateCount_Asset(Item itemAssetReference)
    {
        return GetDuplicateCount(itemAssetReference.GetInstanceID());
    }
    public int GetDuplicateCount_Copy(Item itemCopy)
    {
        return GetDuplicateCount(itemCopy.originalAssetID);
    }
    private int GetDuplicateCount(int originalAssetID)
    {
        int amount = 0;
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].originalAssetID == originalAssetID)
                amount++;
        }
        return amount;
    }

    public void Unequip(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            item.Unequip();

            if (OnItemListChange != null)
                OnItemListChange();
        }
    }
    public void UnequipFirst()
    {
        if(items.Count > 0)
        {
            Unequip(items[0]);
        }
    }

    public override void OnGameReady()
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].OnGameReady();
        }
    }

    public override void OnGameStarted()
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].OnGameStarted();
        }
    }

    void Update()
    {
        if (Game.Instance.gameReady)
            for (int i = 0; i < items.Count; i++)
            {
                items[i].OnUpdate();
            }
    }
}
