﻿using CCC.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FullInspector;

public class LootBoxRef : BaseScriptableObject {

    public string identifiant; // doit etre identique au nom de l'objet
    public Sprite icon;
    public Dictionary<EquipablePreview, int> possibleItems = new Dictionary<EquipablePreview, int>();
    public int amount;
    public StorePrice.CommandType commandType;

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

    public List<EquipablePreview> GetRewards()
    {
        List<EquipablePreview> reward = new List<EquipablePreview>();
        Lottery lot = new Lottery();

        foreach (KeyValuePair<EquipablePreview, int> value in possibleItems)
        {
            lot.Add(new Reward(value.Key, value.Value));
        }

        for (int i = 0; i < amount; i++)
        {
            reward.Add(((Reward)lot.Pick()).equipable);
        }

        return reward;
    }
}
