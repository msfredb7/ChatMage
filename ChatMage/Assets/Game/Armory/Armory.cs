using FullInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Armory : MonoBehaviour {

    // On a des previews qu'on affichera dans le UI pour ensuite 

    // Items
    private int itemSlots;
    public List<EquipablePreview> items = new List<EquipablePreview>(); // catalogue des items

    // Cars
    public List<EquipablePreview> cars = new List<EquipablePreview>(); // catalogue des voitures

    // Smashs
    public List<EquipablePreview> smashes = new List<EquipablePreview>(); // catalogue des smash

    public static int GetLastSavedSlots() { return GameSaves.instance.GetInt(GameSaves.Type.Account, "Slots"); }

    public void Load()
    {
        itemSlots = GameSaves.instance.GetInt(GameSaves.Type.Account, "Slots");
    }

    public void Save()
    {
        GameSaves.instance.SetInt(GameSaves.Type.Account, "Slots", itemSlots);
    }

    public List<EquipablePreview> GetAllUnlockedItems()
    {
        List<EquipablePreview> result = new List<EquipablePreview>();
        for(int i = 0; i < items.Count; i++)
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

    public List<EquipablePreview> GetAllUnlockedSmash()
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
        if(Account.instance.ChangeMoney(amount * slotCost))
        {
            itemSlots += amount;
            return true;
        } else return false;
    }

    public int GetItemSlots()
    {
        return itemSlots;
    }

    public void DebugSetItemSlot(int number)
    {
        itemSlots = number;
    }

    public EquipablePreview DebugGetItem()
    {
        return items[0];
    }

    public EquipablePreview DebugGetSmash()
    {
        return smashes[0];
    }

    public EquipablePreview DebugGetCar()
    {
        return cars[0];
    }
}
