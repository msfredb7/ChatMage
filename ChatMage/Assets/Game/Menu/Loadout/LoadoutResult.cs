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

    public EquipableOrder smashOrder;
    public EquipableOrder carOrder;
    public List<EquipableOrder> itemOrders = new List<EquipableOrder>();

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
                itemOrders.Add(new EquipableOrder(name, type));
                break;
            default:
                throw new System.Exception("Unhandeled equipable type");
        }
    }
}
