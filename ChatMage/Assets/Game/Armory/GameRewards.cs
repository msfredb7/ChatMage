using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using EndGameReward;

public class GameRewards {

    public class GoldReward
    {
        public int baseAmountMin;
        public int baseAmountMax;
        public int firstWinAmountMin;
        public int firstWinAmountMax;
    }

    public bool giveMoney;
    [InspectorShowIf("giveMoney")]
    public GoldReward goldReward;
    public bool giveLootBox;
    [InspectorShowIf("giveLootBox")]
    public List<LootBoxRef> lootboxs = new List<LootBoxRef>();
    public PinataExplosion.BallColor lootboxColor;

    // Ajout de reward de slot ?
    // Ajout de reward d'objet directement ?

    // To open the lootbox : new LootBox(lootboxs[0], delegate (List<EquipablePreview> rewards) { ... });

    public int GetGoldReward(bool firstWin)
    {
        if (!giveMoney)
            return 0;
        if (firstWin)
        {
            int goldAmount = Random.Range(goldReward.baseAmountMin, goldReward.baseAmountMax) + Random.Range(goldReward.firstWinAmountMin, goldReward.firstWinAmountMax);
            Account.instance.Command(StorePrice.CommandType.customGoldAmount, goldAmount);
            return goldAmount;
        }else
        {
            int goldAmount = Random.Range(goldReward.baseAmountMin, goldReward.baseAmountMax);
            Account.instance.Command(StorePrice.CommandType.customGoldAmount, goldAmount);
            return goldAmount;
        }
    }
}
