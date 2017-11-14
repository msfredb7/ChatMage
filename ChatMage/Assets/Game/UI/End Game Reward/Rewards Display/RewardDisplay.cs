using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EndGameReward
{
    public class RewardDisplay : MonoBehaviour
    {

        //public Button nextButton;
        //public Image rewardImage;
        //public Sprite coinsSprite;

        //private int currentReward;

        public Button continueButton;
        public Text levelCleanedText;
        public RewardDisplayItem[] items;

        public void Init(GameReward rewards)
        {
            int baseGold;
            int bonusGold;
            rewards.GetAndApplyGoldReward(out baseGold, out bonusGold);

            // Ajout de toutes les recompenses contenant dans GameRewards dans une seule liste
            if (rewards.firstWin && rewards.HasLootbox())
                LootBox.NewLootbox(rewards.lootboxRefName, delegate (LootBox lootbox)
                {
                    DisplayTemporaire(lootbox, baseGold, bonusGold);
                }, rewards.goldified);
            else
            {
                DisplayTemporaire(null, baseGold, bonusGold);
            }
        }

        public void DisplayTemporaire(LootBox lootbox, int baseGold, int bonusGold)
        {
            int i = 0;
            items[i].Show(baseGold, false);
            i++;

            if (bonusGold > 0)
            {
                items[i].Show(baseGold, true);
                i++;
            }

            if (lootbox != null)
                for (int u = 0; u < lootbox.rewards.Count; u++)
                {
                    if (i >= items.Length)
                        throw new System.Exception("Ben la, ya trop de rewards.");

                    LootBoxRewards reward = lootbox.rewards[u];
                    if (reward.goldAmount > 0)
                        items[i].Show(reward.equipable, reward.goldAmount);
                    else
                        items[i].Show(reward.equipable);
                    i++;
                }

            continueButton.gameObject.SetActive(true);
            levelCleanedText.gameObject.SetActive(true);
        }
    }
}
