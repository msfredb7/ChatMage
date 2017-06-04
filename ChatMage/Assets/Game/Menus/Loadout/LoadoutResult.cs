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
                    return false;
                }

                for (int i = 0; i < itemOrders.Count; i++)
                    if (itemOrders[i].equipableName == name)
                    {
                        throw new System.Exception("Item Already In Loadout");
                        return false;
                    }

                itemOrders.Add(new EquipableOrder(name, type));
                Debug.Log("Item " + name + " added");

                return true;
            default:
                throw new System.Exception("Unhandeled equipable type");
                return false;
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
                return false;
        }
    }
}
