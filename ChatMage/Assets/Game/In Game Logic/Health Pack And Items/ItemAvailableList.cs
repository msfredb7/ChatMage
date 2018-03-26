using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Time Drifter/Item Spawner List")]
public class ItemAvailableList : ScriptableObject
{
    [Header("ITEMS")]
    private List<Item> availableItems = new List<Item>();
    public List<Item> allNonCommonItems = new List<Item>();

    public bool debugUnlockAll = true;

    public void Init()
    {
        availableItems.Clear();
        if (debugUnlockAll)
            availableItems = allNonCommonItems;
        else
        {
            for (int i = 0; i < allNonCommonItems.Count; i++)
            {
                if (allNonCommonItems[i].IsAvailable())
                {
                    availableItems.Add(allNonCommonItems[i]);
                }
            }
        }
    }

    // PEUT METTRE DES ALGO DE SELECTION D'ITEM ICI (ex: Lottery) 
    // -> Ex: mushroom peut etre pogner max 5 fois ?
    // -> dictionnaire ????

    public Item GetRandomAvailableItem()
    {
        return availableItems[Random.Range(0, availableItems.Count-1)];
    }

    public bool AreThereSpecialItemsAvailable()
    {
        return availableItems.Count > 0 ? true : false;
    }
}