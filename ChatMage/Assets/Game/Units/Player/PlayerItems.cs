using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItems : PlayerComponent
{
    public List<Item> items = new List<Item>();
    public event SimpleEvent OnItemListChange;
    public event Action<Item> OnGainItem;
    public event Action<Item> OnLoseItem;

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

        if (OnGainItem != null)
            OnGainItem(newItem);

        if (OnItemListChange != null)
            OnItemListChange();
    }

    public Item GetAReferenceToItemOfType<T>()
    {
        foreach (Item item in items)
        {
            if(item is T)
                return item;
        }
        return null;
    }

    public int GetDuplicateCount_Asset(Item itemAssetReference)
    {
        return GetCountOf(itemAssetReference.GetInstanceID());
    }
    public int GetDuplicateCount_Copy(Item itemCopy)
    {
        return GetCountOf(itemCopy.originalAssetID);
    }
    public int GetCountOf(int originalAssetID)
    {
        int amount = 0;
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].originalAssetID == originalAssetID)
                amount++;
        }
        return amount;
    }
    public int GetCountOf<T>()
    {
        int total = 0;
        foreach (var item in items)
        {
            if (item is T)
                total++;
        }
        return total;
    }

    public void Unequip(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            item.Unequip();

            if (OnLoseItem != null)
                OnLoseItem(item);

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
    public void UnequipAll()
    {
        for (int i = 0; i < items.Count; i++)
        {
            Unequip(items[i]);
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
