using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameRewardAnimation : MonoBehaviour {

    public Button nextButton;
    public Image rewardImage;
    public Sprite coinsSprite;

    //private GameRewards rewards;
    private List<LootBoxRewards> lootboxRewards;

    private int currentReward;

    public void Init(GameRewards rewards, bool firstWin)
    {
        //this.rewards = rewards;

        // Ajout de toutes les recompenses contenant dans GameRewards dans une seule liste
        lootboxRewards = new List<LootBoxRewards>();
        new LootBox(rewards.lootboxs[0], delegate (List<LootBoxRewards> lootboxRewards) 
        {
            for (int i = 0; i < lootboxRewards.Count; i++)
                this.lootboxRewards.Add(lootboxRewards[i]);
        });

        // Ajout de la récompense en or!
        lootboxRewards.Add(new LootBoxRewards(null, rewards.GetGoldReward(firstWin)));

        // On commence par montrer la premiere recompense
        currentReward = 0;
        ShowReward(lootboxRewards[currentReward]);

        // On doit savoir quand le joueur a fini de regarder la recompense courante
        nextButton.onClick.AddListener(OnNextClicked);
    }

    void ShowReward(LootBoxRewards reward)
    {
        if (reward.equipable != null)
        {
            if(reward.cashAmount > 0)
            {
                // doublon
                ShowDuplicate(reward);
            } else
            {
                // nouvel equipement
                ShowEquipable(reward.equipable);
            }
        } else
        {
            // recompense de cash
            ShowMoney(reward.cashAmount);
        }
        nextButton.gameObject.SetActive(true);
        rewardImage.gameObject.SetActive(true);
        currentReward++;
    }

    void ShowEquipable(EquipablePreview equipable)
    {
        rewardImage.sprite = equipable.icon;
    }

    void ShowMoney(int amount)
    {
        rewardImage.sprite = coinsSprite;
        // Faire dequoi avec le montant
    }

    void ShowDuplicate(LootBoxRewards reward)
    {
        rewardImage.sprite = reward.equipable.icon;
        // Faire dequoi avec le montant
    }

    void OnNextClicked()
    {
        // BOUT DE CODE POUR L'ANIMATION ENTRE LES REWARDS
        nextButton.gameObject.SetActive(false);
        rewardImage.gameObject.SetActive(false);
        DelayManager.LocalCallTo(delegate () {
            // Préparation pour la prochaine Reward
            if (currentReward > lootboxRewards.Count - 1)
            {
                // On doit quitter
                LoadingScreen.TransitionTo(LevelSelect.LevelSelection.SCENENAME, null);
            }else if (currentReward > lootboxRewards.Count - 2)
            {
                // Derniere récompense
                nextButton.GetComponentInChildren<Text>().text = "Continue";
                ShowReward(lootboxRewards[currentReward]);
            }
            else
                ShowReward(lootboxRewards[currentReward]);
        }, 1,this);
    }
}
