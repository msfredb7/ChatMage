using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBox {

	public enum LootBoxType
    {
        common = 1,
        rare = 2,
        epic = 3,
        legendary = 4
    }

    private List<EquipablePreview> possibleItems = new List<EquipablePreview>();
    public List<EquipablePreview> rewards = new List<EquipablePreview>();
    private LootBoxType type;

    public LootBox(Armory armory, LootBoxType type)
    {

        possibleItems = armory.GetAllNonAvailablesItems();
        if(possibleItems.Count <= 0)
        {
            Debug.Log("Vous avez tous les items du jeu deja");
            return;
        }
        this.type = type;

        rewards = GetRewards(type);

        for(int i = 0; i < rewards.Count; i++)
        {
            Debug.Log("You got " + rewards[i].displayName);
        }
    }

    private List<EquipablePreview> GetRewards(LootBoxType type)
    {
        List<EquipablePreview> pickedItems = new List<EquipablePreview>();
        for (int i = 0; i < (int)LootBoxType.rare; i++)
        {
            pickedItems.Add(GetRandomItemsWithout(pickedItems));
            pickedItems[pickedItems.Count - 1].available = true;
        }
        return pickedItems;
    }

    private EquipablePreview GetRandomItemsWithout(List<EquipablePreview> pickedItems)
    {
        bool keepGoing = false;
        EquipablePreview result;
        do
        {
            keepGoing = false;
            result = possibleItems[Random.Range(0, possibleItems.Count - 1)];
            for(int i = 0; i < pickedItems.Count; i++)
            {
                if (result == pickedItems[i])
                    keepGoing = true;
            }
        } while (keepGoing);
        return result;
    }
}
