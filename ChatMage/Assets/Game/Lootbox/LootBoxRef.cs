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
    public class LootBoxEquipable
    {
        public string equipablePreviewName;
        public EquipableType type;
        public int weight;

        public LootBoxEquipable(string equipablePreviewName, EquipableType type, int weight)
        {
            this.equipablePreviewName = equipablePreviewName;
            this.type = type;
            this.weight = weight;
        }
    }

    [InspectorHeader("Description")]
    public string identifiant; // doit etre identique au nom de l'objet
    public Sprite icon;
    public PinataExplosion.BallColor lootboxColor;

    [InspectorHeader("Items")]
    public Dictionary<EquipablePreview, int> possibleItems = new Dictionary<EquipablePreview, int>();
    [InspectorButton]
    public void BuildEquipableList()
    {
        items.Clear();
        foreach (KeyValuePair<EquipablePreview, int> value in possibleItems)
            items.Add(new LootBoxEquipable(value.Key.name, value.Key.type, value.Value));
        possibleItems.Clear();
    }
    [InspectorDisabled]
    public List<LootBoxEquipable> items = new List<LootBoxEquipable>();

    public int amount; // Quantité d'items données lors de l'ouverture
    public StorePrice.CommandType commandType; // Type de commande pour le paiement en argent réel

    private List<bool> loadingEventsCompletionList = new List<bool>();

    public class Reward : ILottery
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

    public void LoadAllEquipables(SimpleEvent onComplete)
    {
        loadingEventsCompletionList.Clear();
        possibleItems.Clear();
        foreach (LootBoxEquipable value in items)
        {
            ResourceLoader.LoadEquipablePreviewAsync(value.equipablePreviewName, value.type, delegate (EquipablePreview equipable)
            {
                possibleItems.Add(equipable, value.weight);
                loadingEventsCompletionList.Add(true);
                if (IsLoadingCompleted() && onComplete != null)
                    onComplete.Invoke();
            });
        }
    }

    public bool IsLoadingCompleted()
    {
        if (loadingEventsCompletionList.Count == items.Count) // Liste des items plus le item pour les duplicates
            return true;
        else
            return false;
    }

    public List<EquipablePreview> GetRewards(bool gold)
    {
        List<EquipablePreview> reward = new List<EquipablePreview>();
        Lottery lot = new Lottery();

        foreach (KeyValuePair<EquipablePreview, int> value in possibleItems)
        {
            // Si goldify le lootbox
            if (gold)
            {
                // On peut seulement obtenir des items pas deja unlock
                if (!value.Key.unlocked)
                    lot.Add(new Reward(value.Key, value.Value));
            }
            else
                lot.Add(new Reward(value.Key, value.Value));
        }

        if (lot.Count < 1)
            return reward;

        for (int i = 0; i < amount; i++)
        {
            reward.Add(((Reward)lot.Pick()).equipable);
        }

        return reward;
    }
}
