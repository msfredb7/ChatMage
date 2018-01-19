using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LoadoutResult
{
    public const string LOCAL_KEY = "lr"; // Ce qu'on met devant tous les sauvegarde fait dans le cadre du 'LoadoutResult'
    public const string ITEMCOUNT_KEY = "itCt"; // Ce qu'on met devant tous les sauvegarde fait dans le cadre du 'LoadoutResult'

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

        public void Save(string sufix = "")
        {
            DataSaver.instance.SetString(DataSaver.Type.Loadout, LOCAL_KEY + TypeToTag(type) + sufix, equipableName);
            Debug.Log("Saving '" + LOCAL_KEY + TypeToTag(type) + sufix + "': " + equipableName);
        }
    }

    public EquipableOrder smashOrder;
    public EquipableOrder carOrder;
    public List<EquipableOrder> itemOrders = new List<EquipableOrder>();

    public static LoadoutResult Load(int maxItemCount = int.MaxValue)
    {
        LoadoutResult lastResult = new LoadoutResult();

        //Check smash
        string smashKey = LOCAL_KEY + TypeToTag(EquipableType.Smash);

        if (DataSaver.instance.ContainsString(DataSaver.Type.Loadout, smashKey))
            lastResult.AddEquipable(DataSaver.instance.GetString(DataSaver.Type.Loadout, smashKey), EquipableType.Smash);



        //Check Car
        string carKey = LOCAL_KEY + TypeToTag(EquipableType.Car);

        if (DataSaver.instance.ContainsString(DataSaver.Type.Loadout, carKey))
            lastResult.AddEquipable(DataSaver.instance.GetString(DataSaver.Type.Loadout, carKey), EquipableType.Car);



        //Check Items

        //Check la derniere quantité d'item sauvegardé
        if (DataSaver.instance.ContainsInt(DataSaver.Type.Loadout, ITEMCOUNT_KEY))
            maxItemCount = Mathf.Min(maxItemCount, DataSaver.instance.GetInt(DataSaver.Type.Loadout, ITEMCOUNT_KEY));

        //Continue à check pour des items, tant et aussi longtemps qu'on est < le minimum OU que yen a plus de sauvegardé
        int i = 0;
        while (true)
        {
            string itemKey = LOCAL_KEY + TypeToTag(EquipableType.Item) + i.ToString();

            if (DataSaver.instance.ContainsString(DataSaver.Type.Loadout, itemKey))
                lastResult.AddEquipable(DataSaver.instance.GetString(DataSaver.Type.Loadout, itemKey), EquipableType.Item);
            else
                break;

            i++;
            if (i >= maxItemCount)
                break;
        }

        return lastResult;
    }

    public bool AddEquipable(string name, EquipableType type)
    {
        switch (type)
        {
            case EquipableType.Car:
                carOrder = new EquipableOrder(name, type);
                //Debug.Log("Car " + name + " selected");
                return true;
            case EquipableType.Smash:
                smashOrder = new EquipableOrder(name, type);
                //Debug.Log("Smash " + name + " selected");
                return true;
            case EquipableType.Item:

                for (int i = 0; i < itemOrders.Count; i++)
                    if (itemOrders[i].equipableName == name)
                    {
                        throw new System.Exception("Item Already In Loadout");
                    }

                itemOrders.Add(new EquipableOrder(name, type));
                //Debug.Log("Item " + name + " added");

                return true;
            default:
                throw new System.Exception("Unhandeled equipable type");
        }
    }

    public void Save()
    {
        if (smashOrder != null)
            smashOrder.Save();
        if (carOrder != null)
            carOrder.Save();
        if (itemOrders.Count >= 1)
        {
            for (int i = 0; i < itemOrders.Count; i++)
            {
                itemOrders[i].Save(i.ToString());
            }
        }
        DataSaver.instance.SetInt(DataSaver.Type.Loadout, ITEMCOUNT_KEY, itemOrders.Count);
        DataSaver.instance.SaveData(DataSaver.Type.Loadout);
    }

    /// <summary>
    /// Retourne les clés de sauvegarde utilisé par type d'equipable
    /// </summary>
    private static string TypeToTag(EquipableType type)
    {
        switch (type)
        {
            case EquipableType.Car:
                return "car";
            case EquipableType.Smash:
                return "sm";
            default:
            case EquipableType.Item:
                return "itm";
        }
    }
}
