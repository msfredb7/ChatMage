using FullInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Armory : MonoBehaviour {

    // On a des previews qu'on affichera dans le UI pour ensuite 

    // Items
    private int slots;
    public List<EquipablePreview> items = new List<EquipablePreview>(); // catalogue des items

    // Cars
    public List<EquipablePreview> cars = new List<EquipablePreview>(); // catalogue des voitures

    // Smashs
    public List<EquipablePreview> smashes = new List<EquipablePreview>(); // catalogue des smash

    public static int GetLastSavedSlots() { return PlayerPrefs.GetInt("Slots"); }

    public void Load()
    {
        slots = PlayerPrefs.GetInt("Slots");
    }

    public void Save()
    {
        PlayerPrefs.SetInt("Slots", slots);
        PlayerPrefs.Save();
    }

    public List<EquipablePreview> GetAllAvailablesItems()
    {
        List<EquipablePreview> result = new List<EquipablePreview>();
        for(int i = 0; i < items.Count; i++)
        {
            if (items[i].available == true)
                result.Add(items[i]);
        }
        return result;
    }

    public List<EquipablePreview> GetAllAvailablesCars()
    {
        List<EquipablePreview> result = new List<EquipablePreview>();
        for (int i = 0; i < cars.Count; i++)
        {
            if (cars[i].available == true)
                result.Add(cars[i]);
        }
        return result;
    }

    public List<EquipablePreview> GetAllAvailablesSmash()
    {
        List<EquipablePreview> result = new List<EquipablePreview>();
        for (int i = 0; i < smashes.Count; i++)
        {
            if (smashes[i].available == true)
                result.Add(smashes[i]);
        }
        return result;
    }

    public List<EquipablePreview> GetAllNonAvailablesItems()
    {
        List<EquipablePreview> result = new List<EquipablePreview>();
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].available == false)
                result.Add(items[i]);
        }
        return result;
    }

    public List<EquipablePreview> GetAllNonAvailablesCars()
    {
        List<EquipablePreview> result = new List<EquipablePreview>();
        for (int i = 0; i < cars.Count; i++)
        {
            if (cars[i].available == false)
                result.Add(cars[i]);
        }
        return result;
    }

    public List<EquipablePreview> GetAllNonAvailablesSmash()
    {
        List<EquipablePreview> result = new List<EquipablePreview>();
        for (int i = 0; i < smashes.Count; i++)
        {
            if (smashes[i].available == false)
                result.Add(smashes[i]);
        }
        return result;
    }

    public bool BuySlots(int amount, int slotCost)
    {
        if(Account.instance.ChangeMoney(amount * slotCost))
        {
            slots += amount;
            return true;
        } else return false;
    }
}
