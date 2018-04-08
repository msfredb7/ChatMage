using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Time Drifter/Item Spawner List")]
public class ItemAvailableList : ScriptableObject
{
    [Header("ITEMS")]
    public List<Item> allNonCommonItems = new List<Item>();
    public bool debugUnlockAll = true;

    private List<Item> availableItems = new List<Item>();

    public void Init()
    {
        availableItems.Clear();
        if (debugUnlockAll)
            availableItems.AddRange(allNonCommonItems);
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

    public Item GetRandomAvailableItem()
    {
        Lottery<Item> lotItem = new Lottery<Item>();
        for (int i = 0; i < availableItems.Count; i++)
        {
            // NB: ça devrait être à chaque item de décidé. Il pourrons le faire dans GetWeight
            //if(CanGetMoreOfItem(availableItems[i]))
            lotItem.Add(availableItems[i], availableItems[i].GetWeight());
        }

        if (lotItem.Count > 0 && lotItem.TotalWeight > 0)
        {
            return lotItem.Pick();
        }
        else
        {
            return null;
        }
    }

    public bool AreThereSpecialItemsAvailable()
    {
        return availableItems.Count > 0 ? true : false;
    }

    //private bool CanGetMoreOfItem(Item item)
    //{
    //    List<Item> playerItems = Game.Instance.Player.playerItems.items;
    //    int amountPlayerHas = 0;
    //    for (int i = 0; i < playerItems.Count; i++)
    //    {
    //        if (playerItems[i].GetType() == item.GetType())
    //            amountPlayerHas++;
    //    }

    //    if (item.maxEquiped < 0)
    //        return true;

    //    return amountPlayerHas >= item.maxEquiped ? false : true;
    //}
}