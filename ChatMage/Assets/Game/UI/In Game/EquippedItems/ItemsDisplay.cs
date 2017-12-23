using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemsDisplay : MonoBehaviour
{
    public ItemsDisplay_Item displayPrefab;
    public RectTransform container;

    private List<ItemsDisplay_Item> placedItems = new List<ItemsDisplay_Item>();

    public void Init(PlayerController player)
    {
        player.playerItems.OnItemListChange += () => UpdateDisplay(player.playerItems);
    }

    /// <summary>
    /// TEMPORAIRE
    /// </summary>
    /// <param name="pi"></param>
    void UpdateDisplay(PlayerItems pi)
    {
        if (placedItems == null)
            placedItems = new List<ItemsDisplay_Item>();

        List<Item> itemList = pi.items;

        int i = 0;
        for (; i < itemList.Count; i++)
        {
            if (i >= placedItems.Count)
            {
                placedItems.Add(displayPrefab.DuplicateGO(container));
            }

            placedItems[i].Display(itemList[i]);
        }

        for (; i < placedItems.Count; i++)
        {
            placedItems[i].Hide();
        }
    }
}
