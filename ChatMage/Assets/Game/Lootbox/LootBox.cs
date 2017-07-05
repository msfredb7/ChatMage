using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Objet utile pour généré l'effet d'un lootbox, ceci ne gère pas les animations
public class LootBox
{

    public enum LootBoxType
    {
        small = 1,
        medium = 2,
        large = 3
    }

    public List<LootBoxRef> lootboxes = new List<LootBoxRef>();
    public List<EquipablePreview> possibleItems = new List<EquipablePreview>();

    public LootBox(string identifiant, Action<List<LootBoxRewards>> callback, bool gold = false)
    {
        List<EquipablePreview> equipableRewards = new List<EquipablePreview>();
        List<LootBoxRewards> rewards = new List<LootBoxRewards>();

        // Load la Lootbox Ref
        ResourceLoader.LoadLootBoxRefAsync(identifiant, delegate (LootBoxRef lootbox)
        {
            // Load les Equipables dans la Lootbox Ref
            lootbox.LoadAllEquipables(delegate ()
            {

                equipableRewards.AddRange(lootbox.GetRewards(gold));

                for (int i = 0; i < equipableRewards.Count; i++)
                {
                    if (equipableRewards[i].unlocked) // Le joueur a deja l'item
                    {
                        // On change l'apparence de l'item en double par un item duplicate choisit dans le lootboxRef
                        rewards.Add(new LootBoxRewards(equipableRewards[i], StorePrice.duplicateReward));
                        // Et on ajoute la recompense dans le compte du joueur
                        Account.instance.Command(StorePrice.CommandType.duplicateReward);
                    }
                    else
                    {
                        rewards.Add(new LootBoxRewards(equipableRewards[i], StorePrice.duplicateReward));
                        equipableRewards[i].unlocked = true;
                        equipableRewards[i].Save();
                    }
                }
                GameSaves.instance.SaveData(GameSaves.Type.Armory);

                callback.Invoke(rewards);
            });
        });
    }

    public LootBox(LootBoxRef lootbox, Action<List<LootBoxRewards>> callback, bool gold = false)
    {
        List<EquipablePreview> equipableRewards = new List<EquipablePreview>();
        List<LootBoxRewards> rewards = new List<LootBoxRewards>();

        // Load les Equipables dans la Lootbox Ref
        lootbox.LoadAllEquipables(delegate ()
        {

            equipableRewards.AddRange(lootbox.GetRewards(gold));

            for (int i = 0; i < equipableRewards.Count; i++)
            {
                if (equipableRewards[i].unlocked) // Le joueur a deja l'item
                {
                    // On change l'apparence de l'item en double par un item duplicate choisit dans le lootboxRef
                    rewards.Add(new LootBoxRewards(equipableRewards[i], StorePrice.duplicateReward));
                    // Et on ajoute la recompense dans le compte du joueur
                    Account.instance.Command(StorePrice.CommandType.duplicateReward);
                }
                else
                {
                    rewards.Add(new LootBoxRewards(equipableRewards[i], StorePrice.duplicateReward));
                    equipableRewards[i].unlocked = true;
                    equipableRewards[i].Save();
                }
            }
            GameSaves.instance.SaveData(GameSaves.Type.Armory);

            callback.Invoke(rewards);
        });
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
            for (int i = 0; i < pickedItems.Count; i++)
            {
                if (result == pickedItems[i])
                    keepGoing = true;
            }
        } while (keepGoing);
        return result;
    }
}
