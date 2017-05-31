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

    public void AddEquipable(string name, EquipableType type)
    {
        switch (type)
        {
            case EquipableType.Car:
                carOrder = new EquipableOrder(name, type);
                Debug.Log("Car " + name + " selected");
                break;
            case EquipableType.Smash:
                smashOrder = new EquipableOrder(name, type);
                Debug.Log("Smash " + name + " selected");
                break;
            case EquipableType.Item:
                if (itemOrders.Count >= itemSlotAmount)
                    throw new System.Exception("All item slots full");

                for (int i = 0; i < itemOrders.Count; i++)
                    if (itemOrders[i].equipableName == name)
                        throw new System.Exception("Item Already In Loadout");

                itemOrders.Add(new EquipableOrder(name, type));
                Debug.Log("Item " + name + " added");

                break;
            default:
                throw new System.Exception("Unhandeled equipable type");
        }
    }
}
