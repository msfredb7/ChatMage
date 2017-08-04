using CCC.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FullInspector;
using EndGameReward;

public class LootBoxRef : BaseScriptableObject
{
    // Permet d'enregistrer le type également
    [System.Serializable]
    public class LootBoxEquipable : ILotteryItem
    {
        public string equipablePreviewName;
        public EquipableType type;
        public float weight;

        public LootBoxEquipable(string equipablePreviewName, EquipableType type, float weight)
        {
            this.equipablePreviewName = equipablePreviewName;
            this.type = type;
            this.weight = weight;
        }

        public void LoadAsset(Action<EquipablePreview> callback)
        {
            ResourceLoader.LoadEquipablePreviewAsync(equipablePreviewName, type, callback);
        }

        public float Weight()
        {
            return weight;
        }
    }

    public struct PickReport
    {
        public enum Message { None, GoalNoMoreItems, NoEnoughEqInRef }
        public Message specialMessage;
        public List<EquipablePreview> pickedEquipables;

        public static string MessageToString(Message message)
        {
            switch (message)
            {
                default:
                case Message.None:
                    return "";
                case Message.GoalNoMoreItems:
                    return "You have ran out of equipables to unlock. The lootbox ref might not contain enough equipables.";
                case Message.NoEnoughEqInRef:
                    return "The lootbox ref does not contain enough equipables.";
            }
        }
    }

    [InspectorHeader("Description")]
    public string identifiant; // doit etre identique au nom de l'objet

    [InspectorHeader("Add")]
    public Dictionary<EquipablePreview, float> possibleItems = new Dictionary<EquipablePreview, float>();

    [InspectorButton]
    public void AddToList()
    {
        foreach (KeyValuePair<EquipablePreview, float> value in possibleItems)
            items.Add(new LootBoxEquipable(value.Key.name, value.Key.type, value.Value));
        possibleItems.Clear();
    }

    public List<LootBoxEquipable> items = new List<LootBoxEquipable>();

    public int itemsAmount = 1; // Quantité d'items données lors de l'ouverture
    public StorePrice.CommandType commandType; // Type de commande pour le paiement en argent réel

    private List<bool> loadingEventsCompletionList = new List<bool>();

    public class Reward : ILotteryItem
    {
        public float weight;
        public EquipablePreview equipable;

        public Reward(EquipablePreview equipable, float weight)
        {
            this.weight = weight;
            this.equipable = equipable;
        }

        public float Weight()
        {
            return weight;
        }
    }
    public bool IsLoadingCompleted()
    {
        if (loadingEventsCompletionList.Count == items.Count) // Liste des items plus le item pour les duplicates
            return true;
        else
            return false;
    }

    public void LoadAssets(List<LootBoxEquipable> equipables, Action<List<EquipablePreview>> callback)
    {
        List<EquipablePreview> assetList = new List<EquipablePreview>();

        //Est-ce que la liste est vide ?
        if (equipables.Count == 0)
        {
            callback(assetList);
        }
        else
        {
            foreach (LootBoxEquipable value in equipables)
            {
                value.LoadAsset(delegate (EquipablePreview asset)
                {
                    assetList.Add(asset);
                    if (assetList.Count == equipables.Count)
                        callback(assetList);
                });
            }
        }
    }

    public void PickRewards(bool goldified, Action<PickReport> callback)
    {
        // Contruction de la lottery
        Lottery lot = new Lottery();
        bool accessToSmash = Armory.HasAccessToSmash();
        bool accessToItems = Armory.HasAccessToItems();
        for (int i = 0; i < items.Count; i++)
        {
            //Condition: goal + unlocked
            if (goldified && EquipablePreview.IsUnlocked(items[i].equipablePreviewName))
                continue;

            //Condition: access to items
            if (items[i].type == EquipableType.Item && !accessToItems)
                continue;

            //Condition: access to smash
            if (items[i].type == EquipableType.Smash && !accessToSmash)
                continue;

            //C bon, on a clear les conditions !
            lot.Add(items[i]);
        }

        List<LootBoxEquipable> pickedItems = new List<LootBoxEquipable>();

        //On pick
        for (int i = 0; i < itemsAmount; i++)
        {
            if (lot.Count <= 0)
                break;

            int pickedIndex = -1;
            pickedItems.Add(lot.Pick(out pickedIndex) as LootBoxEquipable);
            lot.RemoveAt(pickedIndex);
        }

        LoadAssets(pickedItems, delegate (List<EquipablePreview> loadedEquipables)
        {
            PickReport report = new PickReport()
            {
                pickedEquipables = loadedEquipables,
                specialMessage = PickReport.Message.None
            };

            if(pickedItems.Count < itemsAmount)
            {
                report.specialMessage = goldified ? PickReport.Message.GoalNoMoreItems : PickReport.Message.NoEnoughEqInRef;
            }

            callback(report);
        });
    }
}
