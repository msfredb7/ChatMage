using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Time Drifter/Item Spawner Settings")]
public class ItemSpawnerSettings : ScriptableObject
{
    [Header("Item List")]
    public ItemAvailableList itemList;
    public List<Item> commonItems = new List<Item>();

    [Header("Gain Item")]
    public int gainItemOnKillChance = 25; // / 100
    public float gainItemOnKillHardness = 0.25f;

    [Header("Gain Special Item")]
    public int gainSpecialItemOnItemPackChance = 33;  // / 100
    public float gainItemOnItemPackHardness = 0.25f;

    public Item GainItem()
    {
        return commonItems[0];
    }

    public Item GainSpecialItem()
    {
        return itemList.GetRandomAvailableItem();
    }

    public void Init()
    {
        itemList.Init();
    }
}
