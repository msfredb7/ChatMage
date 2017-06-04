using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LoadoutResult
{
    [System.Serializable]
    public class EquipableOrder
    {
        public EquipableOrder(string equipableName, EquipableType type)
        {
            this.equipableName = equipableName;
            this.type = type;
        }
        public string equipableName;
        public EquipableType type;

        public void Save(int ID = 0)
        {
            GameSaves.instance.SetString(GameSaves.Type.Loadout, "equipableName" + type + "" + ID, equipableName);
            Debug.Log("Saving " + "equipableName" + type + "" + ID);
        }
    }

    public LoadoutResult(int itemSlotAmount)
    {
        this.itemSlotAmount = itemSlotAmount;
    }

    public EquipableOrder smashOrder;
    public EquipableOrder carOrder;
    public List<EquipableOrder> itemOrders = new List<EquipableOrder>();

    public int itemSlotAmount;

    public bool AddEquipable(string name, EquipableType type)
    {
        switch (type)
        {
            case EquipableType.Car:
                carOrder = new EquipableOrder(name, type);
                Debug.Log("Car " + name + " selected");
                return true;
            case EquipableType.Smash:
                smashOrder = new EquipableOrder(name, type);
                Debug.Log("Smash " + name + " selected");
                return true;
            case EquipableType.Item:
                if (itemOrders.Count >= itemSlotAmount)
                {
                    throw new System.Exception("All item slots full");
                }

                for (int i = 0; i < itemOrders.Count; i++)
                    if (itemOrders[i].equipableName == name)
                    {
                        throw new System.Exception("Item Already In Loadout");
                    }

                itemOrders.Add(new EquipableOrder(name, type));
                Debug.Log("Item " + name + " added");

                return true;
            default:
                throw new System.Exception("Unhandeled equipable type");
        }
    }

    public bool AlreadyEquip(string equipableName, EquipableType type)
    {
        switch (type)
        {
            case EquipableType.Car:
                if (carOrder == null)
                    return false;
                if (carOrder.equipableName == equipableName)
                    return true;
                else
                    return false;
            case EquipableType.Smash:
                if (smashOrder == null)
                    return false;
                if (smashOrder.equipableName == equipableName)
                    return true;
                else
                    return false;
            case EquipableType.Item:
                if (itemOrders.Count < 1)
                    return false;
                for (int i = 0; i < itemOrders.Count; i++)
                {
                    if (itemOrders[i].equipableName == equipableName)
                        return true;
                }
                return false;
            default:
                throw new System.Exception("Unhandeled equipable type");
        }
    }

    public void Save()
    {
        if(smashOrder != null)
            smashOrder.Save();
        if(carOrder != null)
            carOrder.Save();
        if (itemOrders.Count >= 1)
        {
            for (int i = 0; i < itemOrders.Count; i++)
            {
                itemOrders[i].Save();
            }
        }
        GameSaves.instance.SaveData(GameSaves.Type.Loadout);
    }

    public void Load()
    {
        if (GameSaves.instance.ContainsString(GameSaves.Type.Loadout, "equipableNameSmash0"))
            AddEquipable(GameSaves.instance.GetString(GameSaves.Type.Loadout, "equipableNameSmash0"), EquipableType.Smash);
        if (GameSaves.instance.ContainsString(GameSaves.Type.Loadout, "equipableNameCar0"))
            AddEquipable(GameSaves.instance.GetString(GameSaves.Type.Loadout, "equipableNameCar0"), EquipableType.Car);
        for (int i = 0; i < itemSlotAmount; i++)
        {
            if (GameSaves.instance.ContainsString(GameSaves.Type.Loadout, "equipableNameItem" + i))
                AddEquipable(GameSaves.instance.GetString(GameSaves.Type.Loadout, "equipableNameItem" + i), EquipableType.Item);
        }
    }
}
