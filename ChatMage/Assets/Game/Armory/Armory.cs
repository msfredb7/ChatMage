using FullInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Armory : BaseScriptableObject
{
    private const string SLOTS_KEY = "slt";
    // On a des previews qu'on affichera dans le UI pour ensuite 

    // Items
    private int itemSlots;
    public int defaultItemSlots = 3;
    public EquipablePreview[] items; // catalogue des items

    // Cars
    public EquipablePreview[] cars; // catalogue des voitures

    // Smashs
    public EquipablePreview[] smashes; // catalogue des smash
    public EquipablePreview defaultSmash;

    public int GetLastSavedSlots()
    {
        if (DataSaver.instance.ContainsInt(DataSaver.Type.Armory, SLOTS_KEY))
            return DataSaver.instance.GetInt(DataSaver.Type.Armory, SLOTS_KEY);
        else
        {
            DataSaver.instance.SetInt(DataSaver.Type.Armory, SLOTS_KEY, defaultItemSlots);
            return defaultItemSlots;
        }
    }

    public void Load()
    {
        itemSlots = GetLastSavedSlots();
        LoadEquipable();
    }

    public void SaveSlot(bool saveToDisk = true)
    {
        DataSaver.instance.SetInt(DataSaver.Type.Armory, SLOTS_KEY, itemSlots);
        if(saveToDisk)
            DataSaver.instance.SaveData(DataSaver.Type.Armory);
    }

    public List<EquipablePreview> GetAllEquipables()
    {
        List<EquipablePreview> result = new List<EquipablePreview>(items.Length + cars.Length + smashes.Length);

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

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].Unlocked)
                result.Add(items[i]);
        }
        for (int i = 0; i < cars.Length; i++)
        {
            if (cars[i].Unlocked)
                result.Add(cars[i]);
        }
        for (int i = 0; i < smashes.Length; i++)
        {
            if (smashes[i].Unlocked)
                result.Add(smashes[i]);
        }
        return result;
    }

    public List<EquipablePreview> GetAllLockedEquipables()
    {
        List<EquipablePreview> result = new List<EquipablePreview>();

        for (int i = 0; i < items.Length; i++)
        {
            if (!items[i].Unlocked)
                result.Add(items[i]);
        }
        for (int i = 0; i < cars.Length; i++)
        {
            if (!cars[i].Unlocked)
                result.Add(cars[i]);
        }
        for (int i = 0; i < smashes.Length; i++)
        {
            if (!smashes[i].Unlocked)
                result.Add(smashes[i]);
        }
        return result;
    }

    public List<EquipablePreview> GetAllUnlockedItems()
    {
        List<EquipablePreview> result = new List<EquipablePreview>();
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].Unlocked == true)
                result.Add(items[i]);
        }
        return result;
    }

    public List<EquipablePreview> GetAllUnlockedCars()
    {
        List<EquipablePreview> result = new List<EquipablePreview>();
        for (int i = 0; i < cars.Length; i++)
        {
            if (cars[i].Unlocked == true)
                result.Add(cars[i]);
        }
        return result;
    }

    public List<EquipablePreview> GetAllUnlockedSmashes()
    {
        List<EquipablePreview> result = new List<EquipablePreview>();
        for (int i = 0; i < smashes.Length; i++)
        {
            if (smashes[i].Unlocked == true)
                result.Add(smashes[i]);
        }
        return result;
    }

    public List<EquipablePreview> GetAllLockedItems()
    {
        List<EquipablePreview> result = new List<EquipablePreview>();
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].Unlocked == false)
                result.Add(items[i]);
        }
        return result;
    }

    public List<EquipablePreview> GetAllLockedCars()
    {
        List<EquipablePreview> result = new List<EquipablePreview>();
        for (int i = 0; i < cars.Length; i++)
        {
            if (cars[i].Unlocked == false)
                result.Add(cars[i]);
        }
        return result;
    }

    public List<EquipablePreview> GetAllLockedSmash()
    {
        List<EquipablePreview> result = new List<EquipablePreview>();
        for (int i = 0; i < smashes.Length; i++)
        {
            if (smashes[i].Unlocked == false)
                result.Add(smashes[i]);
        }
        return result;
    }

    public void AddItemsSlots(int amount, bool saveAfterwards = true)
    {
        itemSlots += amount;

        SaveSlot();
    }

    public int ItemSlots
    {
        get
        {
            return itemSlots;
        }
    }

    public void LoadEquipable()
    {
        List<EquipablePreview> result = new List<EquipablePreview>(items.Length + cars.Length + smashes.Length);

        result.AddRange(items);
        result.AddRange(cars);
        result.AddRange(smashes);

        for (int i = 0; i < result.Count; i++)
        {
            result[i].Load();
        }
    }

    private const string SMASH_ACCESS_KEY = "smACCESS";
    private const string ITEM_ACCESS_KEY = "itACCESS";

    public static bool HasAccessToSmash()
    {
        return DataSaver.instance.GetBool(DataSaver.Type.Armory, SMASH_ACCESS_KEY, false);
    }
    public static bool HasAccessToItems()
    {
        try
        {
            return DataSaver.instance.GetBool(DataSaver.Type.Armory, ITEM_ACCESS_KEY);
        }
        catch
        {
            return false;
        }
    }

    public static void UnlockAccessToSmash()
    {
        DataSaver.instance.SetBool(DataSaver.Type.Armory, SMASH_ACCESS_KEY, true);
        DataSaver.instance.SaveData(DataSaver.Type.Armory);
    }
    public static void LockAccessToSmash()
    {
        DataSaver.instance.SetBool(DataSaver.Type.Armory, SMASH_ACCESS_KEY, false);
        DataSaver.instance.SaveData(DataSaver.Type.Armory);
    }
    public static void UnlockAccessToItems()
    {
        DataSaver.instance.SetBool(DataSaver.Type.Armory, ITEM_ACCESS_KEY, true);
        DataSaver.instance.SaveData(DataSaver.Type.Armory);
    }

}
