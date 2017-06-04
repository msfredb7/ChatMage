using CCC.Manager;
using CompleteProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
    public const string SCENENAME = "Shop";

    public GameObject deactivateScenePanel;
    public Armory armory;

    public Button smallLootbox;
    public Button mediumLootbox;
    public Button largeLootbox;

    void Start()
    {
        MasterManager.Sync(OnSync);
        deactivateScenePanel.SetActive(false);
    }

    void OnSync()
    {
        if (Account.instance == null)
            Scenes.Load("MainMenu");
        smallLootbox.onClick.AddListener(delegate() { BuyLootBox(LootBox.LootBoxType.small); });
        mediumLootbox.onClick.AddListener(delegate () { BuyLootBox(LootBox.LootBoxType.medium); });
        largeLootbox.onClick.AddListener(delegate () { BuyLootBox(LootBox.LootBoxType.large); });
    }

    public void BuyLootBox(LootBox.LootBoxType type)
    {
        PopUpMenu.ShowOKPopUpMenu("Are you sure you want to buy a " + type + " lootbox ?", delegate ()
        {
            // Animation apparition Lootbox
            switch (type)
            {
                case LootBox.LootBoxType.small:
                    new LootBox(armory, LootBox.LootBoxType.small, delegate (List<EquipablePreview> rewards) {
                        // Disparition du Lootbox
                        for (int i = 0; i < rewards.Count; i++)
                        {
                            // Afficher recompense ?
                            //rewards[i]
                        }
                    });
                    break;
                case LootBox.LootBoxType.medium:
                    new LootBox(armory, LootBox.LootBoxType.medium, delegate (List<EquipablePreview> rewards) {
                        // Disparition du Lootbox
                        for (int i = 0; i < rewards.Count; i++)
                        {
                            // Afficher recompense ?
                            //rewards[i]
                        }
                    });
                    break;
                case LootBox.LootBoxType.large:
                    new LootBox(armory, LootBox.LootBoxType.large, delegate (List<EquipablePreview> rewards) {
                        // Disparition du Lootbox
                        for (int i = 0; i < rewards.Count; i++)
                        {
                            // Afficher recompense ?
                            //rewards[i]
                        }
                    });
                    break;
            }
        });
    }

    // TODO: Deplacer dans loadout tab item
    public void BuySlots()
    {
        PopUpMenu.ShowOKPopUpMenu("Are you sure you want to buy an extra slots for items ?", delegate ()
        {
            if ((Account.instance.GetMoney() - 10) < 0)
                PopUpMenu.ShowOKPopUpMenu("You don't have enough money. Open loot boxes or win levels to gain money. See you later!");
            else
                armory.BuyItemSlots(1, -10);
        });
    }

    public void GetMoney()
    {
        Account.instance.ChangeMoney(10);
    }

    public void BuyMoney(int amount)
    {
        GetComponent<Purchaser>().BuyConsumable(amount);
    }

    public void BackButton()
    {
        Scenes.UnloadAsync(SCENENAME);
    }

    public void ShowRewardedAd()
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            deactivateScenePanel.SetActive(true);
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
        } else
        {
            PopUpMenu.ShowOKPopUpMenu("You are not connected to the internet. Please verify your connection and come back again.");
        }
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                //
                // YOUR CODE TO REWARD THE GAMER
                // Give coins etc.
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                //
                // YOUR CODE TO SAY FUCK OFF TO PLAYER
                // Give shit etc.
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                //
                // YOUR CODE TO SAY BEAT THE ASS OF THE PLAYER
                // Give handicap etc.
                break;
        }
        deactivateScenePanel.SetActive(false);
    }
}
