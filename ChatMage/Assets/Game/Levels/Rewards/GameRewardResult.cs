using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRewardResult : IReward
{
    public class Doublon
    {
        public string equipableAssetName;
        public int goldValue;
    }

    public int baseGold = 0;
    public int firstWinGold = 0;
    public List<string> newEquipableAssetNames = null;
    public List<Doublon> doublons = null;

    public int GoldAmount()
    {
        int total = baseGold + firstWinGold;

        if (doublons != null)
            for (int i = 0; i < doublons.Count; i++)
            {
                total += doublons[i].goldValue;
            }

        return total;
    }

    public string[] EquipableAssetsName()
    {
        return newEquipableAssetNames.ToArray();
    }
}
