using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Time Drifter/Item Spawner Settings")]
public class ItemSpawnerSettings : ScriptableObject
{
    public List<Item> commonItems = new List<Item>();
    public List<Item> specialItems = new List<Item>();

    [Header("Gain Item")]
    public int everyXKill = 10;

    [Header("Gain Special Item")]
    public int specialItemEveryXItem = 3;
}
