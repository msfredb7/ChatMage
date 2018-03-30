using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Time Drifter/Item Spawner Settings")]
public class ItemSpawnerSettings : ScriptableObject
{
    [Header("Item List")]
    public ItemAvailableList itemList;
    public List<Item> commonItems = new List<Item>();

    [Header("Gain Item"), Range(0, 100)]
    public int gainItemOnKillChance = 25; // / 100
    [Range(0, 1)]
    public float gainItemOnKillHardness = 0.25f;

    [Header("Gain Special Item"), Range(0, 100)]
    public int gainSpecialItemOnItemPackChance = 33;  // / 100
    [Range(0, 1)]
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
