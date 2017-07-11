using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

public class GameReward
{

    public class GoldReward
    {
        public int baseAmountMin;
        public int baseAmountMax;
        public int firstWinAmountMin;
        public int firstWinAmountMax;
    }

    public GoldReward goldReward;
    public bool giveLootBox;
    [InspectorShowIf("giveLootBox")]
    public string lootboxRefName;
    [InspectorShowIf("giveLootBox")]
    public bool goldified = false;

    // Ajout de reward de slot ?

    // To open the lootbox : new LootBox(lootboxs[0], delegate (List<EquipablePreview> rewards) { ... });

    public bool HasLootbox()
    {
        return giveLootBox && lootboxRefName != "";
    }

    public void GetAndApplyGoldReward(bool firstWin, out int baseGold, out int bonusGold)
    {
        baseGold = Random.Range(goldReward.baseAmountMin, goldReward.baseAmountMax + 1);
        bonusGold = firstWin ? Random.Range(goldReward.firstWinAmountMin, goldReward.firstWinAmountMax + 1) : 0;

        int total = baseGold + bonusGold;
        Account.instance.Command(StorePrice.CommandType.customGoldAmount, total);
    }
}
