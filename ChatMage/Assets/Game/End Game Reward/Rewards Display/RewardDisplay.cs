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
        public RewardDisplayItem[] items;

        public void Init(GameReward rewards, bool firstWin)
        {
            int baseGold;
            int bonusGold;
            rewards.GetAndApplyGoldReward(firstWin, out baseGold, out bonusGold);

            // Ajout de toutes les recompenses contenant dans GameRewards dans une seule liste
            if (rewards.HasLootbox())
                LootBox.NewLootbox(rewards.lootboxRefName, delegate (LootBox lootbox)
                {
                    DisplayTemporaire(lootbox, baseGold, bonusGold);
                }, rewards.goldified);
            else
            {
                DisplayTemporaire(null, baseGold, bonusGold);
            }

            //// Ajout de la récompense en or! (TODO : A CHANGER, LA RECOMPENSE D'OR SERA AFFICHÉ DIFFÉREMMENT)
            //lootboxRewards.Add(new LootBoxRewards(null, rewards.GetGoldReward(firstWin)));

            //// On commence par montrer la premiere recompense
            //currentReward = 0;
            //ShowReward(lootboxRewards[currentReward]);

            // On doit savoir quand le joueur a fini de regarder la recompense courante
            //nextButton.onClick.AddListener(OnNextClicked);
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
        }

        //void ShowReward(LootBoxRewards reward)
        //{
        //    if (reward.equipable != null)
        //    {
        //        if(reward.cashAmount > 0)
        //        {
        //            // doublon
        //            ShowDuplicate(reward);
        //        } else
        //        {
        //            // nouvel equipement
        //            ShowEquipable(reward.equipable);
        //        }
        //    } else
        //    {
        //        // recompense de cash
        //        ShowMoney(reward.cashAmount);
        //    }
        //    nextButton.gameObject.SetActive(true);
        //    rewardImage.gameObject.SetActive(true);
        //    currentReward++;
        //}

        //void ShowEquipable(EquipablePreview equipable)
        //{
        //    rewardImage.sprite = equipable.icon;
        //}

        //void ShowMoney(int amount)
        //{
        //    rewardImage.sprite = coinsSprite;
        //    // Faire dequoi avec le montant
        //}

        //void ShowDuplicate(LootBoxRewards reward)
        //{
        //    rewardImage.sprite = reward.equipable.icon;
        //    // Faire dequoi avec le montant
        //}

        //void OnNextClicked()
        //{
        //    // BOUT DE CODE POUR L'ANIMATION ENTRE LES REWARDS
        //    nextButton.gameObject.SetActive(false);
        //    rewardImage.gameObject.SetActive(false);
        //    DelayManager.LocalCallTo(delegate () {
        //        // Préparation pour la prochaine Reward
        //        if (currentReward > lootboxRewards.Count - 1)
        //        {
        //            // On doit quitter
        //            LoadingScreen.TransitionTo(LevelSelect.LevelSelection.SCENENAME, null);
        //        }else if (currentReward > lootboxRewards.Count - 2)
        //        {
        //            // Derniere récompense
        //            nextButton.GetComponentInChildren<Text>().text = "Continue";
        //            ShowReward(lootboxRewards[currentReward]);
        //        }
        //        else
        //            ShowReward(lootboxRewards[currentReward]);
        //    }, 1,this);
        //}
    }
}
