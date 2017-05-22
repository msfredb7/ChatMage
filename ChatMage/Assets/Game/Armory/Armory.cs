using FullInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Armory : MonoBehaviour {

    // On a des previews qu'on affichera dans le UI pour ensuite 

    // Money
    private int money;
    public UnityEvent onMoneyChanged = new UnityEvent();

    // Items
    private int slots;
    public List<EquipablePreview> items = new List<EquipablePreview>(); // catalogue des items

    // Cars
    public List<EquipablePreview> cars = new List<EquipablePreview>(); // catalogue des voitures

    // Smashs
    public List<EquipablePreview> smashes = new List<EquipablePreview>(); // catalogue des smash

    public static int GetLastSavedMoney() { return PlayerPrefs.GetInt("Money"); }
    public static int GetLastSavedSlots() { return PlayerPrefs.GetInt("Slots"); }

    public void Load()
    {
        money = PlayerPrefs.GetInt("Money");
        slots = PlayerPrefs.GetInt("Slots");
    }

    public void Save()
    {
        PlayerPrefs.SetInt("Money",money);
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

    /// <summary>
    /// Ajout ou retire un certain montant d'argent au compte du joueur
    /// </summary>
    /// <returns>Retourne si le changement a reussi ou pas</returns>
    public bool ChangeMoney(int amount)
    {
        int moneyResult = money - amount;
        if (moneyResult < 0)
            return false;
        else money = moneyResult;
        onMoneyChanged.Invoke();
        return true;
    }

    public bool BuySlots(int amount, int slotCost)
    {
        if(ChangeMoney(amount * slotCost))
        {
            slots += amount;
            return true;
        } else return false;
    }
}
