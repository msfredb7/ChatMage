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

    private string previousScene;
    private LoadoutTab.LoadoutTab_Type previousTab;
    private string previousLevel;

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
        if (previousScene == Loadout.SCENENAME)
            LoadingScreen.TransitionTo(previousScene,new ToLoadoutMessage(previousScene,previousTab));
        else
            LoadingScreen.TransitionTo(previousScene,null);
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
                Account.instance.ChangeMoney(10);
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                //
                // YOUR CODE TO SAY FUCK OFF TO PLAYER
                // Give shit etc.
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                PopUpMenu.ShowPopUpMenu("A problem has occured and the ad could not be shown.",2);
                break;
        }
        deactivateScenePanel.SetActive(false);
    }

    public void SetPreviousContext(string previousScene, LoadoutTab.LoadoutTab_Type previousTab, string previousLevel = null)
    {
        this.previousScene = previousScene;
        this.previousTab = previousTab;
        this.previousLevel = previousLevel;
    }
}
