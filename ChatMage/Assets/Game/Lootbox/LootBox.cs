using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Objet utile pour généré l'effet d'un lootbox, ceci ne gère pas les animations
public class LootBox {

	public enum LootBoxType
    {
        small = 1,
        medium = 2,
        large = 3
    }

    public List<LootBoxRef> lootboxes = new List<LootBoxRef>();
    public List<EquipablePreview> possibleItems = new List<EquipablePreview>();

    /// <summary>
    /// Deprecated | Ancienne fonction pour ouvrir une lootbox
    /// </summary>
    public LootBox(Armory armory, LootBoxType type, Action<List<EquipablePreview>> callback, bool gold = false)
    {
        List<EquipablePreview> rewards = new List<EquipablePreview>();

        // Calcul des objets possibles
        if (!gold)
            possibleItems = armory.GetAllEquipables();
        else
            possibleItems = armory.GetAllEquipablesLock();

        if(possibleItems.Count <= 0)
        {
            Debug.Log("Vous avez tous les items du jeu deja");
            callback.Invoke(null);
            return;
        }

        rewards = GetRewards(type);

        for(int i = 0; i < rewards.Count; i++)
            Debug.Log("You got " + rewards[i].displayName);

        callback.Invoke(rewards);
    }

    public LootBox(string identifiant, Action<List<EquipablePreview>> callback)
    {
        List<EquipablePreview> rewards = new List<EquipablePreview>();
        
        ResourceLoader.LoadLootBoxRefAsync(identifiant, delegate (LootBoxRef lootbox) { rewards.AddRange(lootbox.GetRewards()); });

        for (int i = 0; i < rewards.Count; i++)
        {
            string completedUnlockKey = Armory.SAVE_PREFIX + rewards[i].equipableAssetName;
            GameSaves.instance.SetBool(GameSaves.Type.Armory, completedUnlockKey, true);
        }

        callback.Invoke(rewards);
    }

    private List<EquipablePreview> GetRewards(LootBoxType type)
    {
        List<EquipablePreview> pickedItems = new List<EquipablePreview>();
        for (int i = 0; i < (int)LootBoxType.medium; i++)
        {
            pickedItems.Add(GetRandomItemsWithout(pickedItems));
            pickedItems[pickedItems.Count - 1].unlocked = true;
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
            result = possibleItems[UnityEngine.Random.Range(0, possibleItems.Count - 1)];
            for(int i = 0; i < pickedItems.Count; i++)
            {
                if (result == pickedItems[i])
                    keepGoing = true;
            }
        } while (keepGoing);
        return result;
    }
}
