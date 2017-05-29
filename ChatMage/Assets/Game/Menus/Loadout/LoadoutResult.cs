using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadoutResult
{
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
                break;
            case EquipableType.Smash:
                smashOrder = new EquipableOrder(name, type);
                break;
            case EquipableType.Item:
                for (int i = 0; i < itemOrders.Count; i++)
                {
                    if(itemOrders[i].equipableName == name)
                    {
                        throw new System.Exception("Item Already In Loadout");
                    }
                }
                itemOrders.Add(new EquipableOrder(name, type));
                break;
            default:
                throw new System.Exception("Unhandeled equipable type");
        }
    }
}
