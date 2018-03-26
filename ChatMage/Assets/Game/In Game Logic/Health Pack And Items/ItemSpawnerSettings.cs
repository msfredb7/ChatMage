using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Time Drifter/Item Spawner Settings")]
public class ItemSpawnerSettings : ScriptableObject
{
    public ItemAvailableList itemList;
    public List<Item> commonItems = new List<Item>();

    [Header("Gain Item")]
    public int everyXKill = 10;

    [Header("Gain Special Item")]
    public int specialItemEveryXItem = 3;

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
