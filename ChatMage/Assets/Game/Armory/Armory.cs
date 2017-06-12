using FullInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Armory : ScriptableObject
{
    private const string SLOTS_KEY = "slt";
    // On a des previews qu'on affichera dans le UI pour ensuite 

    // Items
    private int itemSlots;
    public int defaultItemSlots = 3;
    public List<EquipablePreview> items = new List<EquipablePreview>(); // catalogue des items

    // Cars
    public List<EquipablePreview> cars = new List<EquipablePreview>(); // catalogue des voitures

    // Smashs
    public List<EquipablePreview> smashes = new List<EquipablePreview>(); // catalogue des smash

    public int GetLastSavedSlots()
    {
        if (GameSaves.instance.ContainsInt(GameSaves.Type.Armory, SLOTS_KEY))
            return GameSaves.instance.GetInt(GameSaves.Type.Armory, SLOTS_KEY);
        else
        {
            GameSaves.instance.SetInt(GameSaves.Type.Armory, SLOTS_KEY, defaultItemSlots);
            return defaultItemSlots;
        }
    }

    public void Load()
    {
        itemSlots = GetLastSavedSlots();
    }

    public void Save()
    {
        GameSaves.instance.SetInt(GameSaves.Type.Armory, SLOTS_KEY, itemSlots);
    }

    public List<EquipablePreview> GetAllEquipables()
    {
        List<EquipablePreview> result = new List<EquipablePreview>(items.Count + cars.Count + smashes.Count);

        result.AddRange(items);
        result.AddRange(cars);
        result.AddRange(smashes);

        return result;
    }

    public List<EquipablePreview> GetAllEquipablesLock()
    {
        List<EquipablePreview> result = new List<EquipablePreview>();

        result.AddRange(GetAllLockedItems());
        result.AddRange(GetAllLockedCars());
        result.AddRange(GetAllLockedSmash());

        return result;
    }

    public List<EquipablePreview> GetAllUnlockedEquipables()
    {
        List<EquipablePreview> result = new List<EquipablePreview>();

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].unlocked)
                result.Add(items[i]);
        }
        for (int i = 0; i < cars.Count; i++)
        {
            if (cars[i].unlocked)
                result.Add(cars[i]);
        }
        for (int i = 0; i < smashes.Count; i++)
        {
            if (smashes[i].unlocked)
                result.Add(smashes[i]);
        }
        return result;
    }

    public List<EquipablePreview> GetAllLockedEquipables()
    {
        List<EquipablePreview> result = new List<EquipablePreview>();

        for (int i = 0; i < items.Count; i++)
        {
            if (!items[i].unlocked)
                result.Add(items[i]);
        }
        for (int i = 0; i < cars.Count; i++)
        {
            if (!cars[i].unlocked)
                result.Add(cars[i]);
        }
        for (int i = 0; i < smashes.Count; i++)
        {
            if (!smashes[i].unlocked)
                result.Add(smashes[i]);
        }
        return result;
    }

    public List<EquipablePreview> GetAllUnlockedItems()
    {
        List<EquipablePreview> result = new List<EquipablePreview>();
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].unlocked == true)
                result.Add(items[i]);
        }
        return result;
    }

    public List<EquipablePreview> GetAllUnlockedCars()
    {
        List<EquipablePreview> result = new List<EquipablePreview>();
        for (int i = 0; i < cars.Count; i++)
        {
            if (cars[i].unlocked == true)
                result.Add(cars[i]);
        }
        return result;
    }

    public List<EquipablePreview> GetAllUnlockedSmashes()
    {
        List<EquipablePreview> result = new List<EquipablePreview>();
        for (int i = 0; i < smashes.Count; i++)
        {
            if (smashes[i].unlocked == true)
                result.Add(smashes[i]);
        }
        return result;
    }

    public List<EquipablePreview> GetAllLockedItems()
    {
        List<EquipablePreview> result = new List<EquipablePreview>();
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].unlocked == false)
                result.Add(items[i]);
        }
        return result;
    }

    public List<EquipablePreview> GetAllLockedCars()
    {
        List<EquipablePreview> result = new List<EquipablePreview>();
        for (int i = 0; i < cars.Count; i++)
        {
            if (cars[i].unlocked == false)
                result.Add(cars[i]);
        }
        return result;
    }

    public List<EquipablePreview> GetAllLockedSmash()
    {
        List<EquipablePreview> result = new List<EquipablePreview>();
        for (int i = 0; i < smashes.Count; i++)
        {
            if (smashes[i].unlocked == false)
                result.Add(smashes[i]);
        }
        return result;
    }

    public bool BuyItemSlots(int amount, int slotCost)
    {
        if (Account.instance.AddCoins(amount * slotCost))
        {
            itemSlots += amount;
            return true;
        }
        else return false;
    }

    public int ItemSlots
    {
        get
        {
            return itemSlots;
        }
    }
}
