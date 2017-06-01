using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class ShopMenu : MonoBehaviour
{
    public GameObject deactivateScenePanel;

    void Start()
    {
        MasterManager.Sync(OnSync);
        deactivateScenePanel.SetActive(false);
    }

    void OnSync()
    {
        if (Account.instance == null)
            Scenes.Load("MainMenu");
    }

    public void BuyLootBox()
    {
        // Animation apparition Lootbox

        new LootBox(Account.instance.armory, LootBox.LootBoxType.small, delegate(List<EquipablePreview> rewards) {
            // Disparition du Lootbox
            for (int i = 0; i < rewards.Count; i++)
            {
                // Afficher recompense ?
                //rewards[i]
            }
        });
    }

    public void BuySlots()
    {
        if((Account.instance.GetMoney() - 10) < 0)
            PopUpMenu.ShowOKPopUpMenu("You don't have enough money. Open loot boxes or win levels to gain money. See you later!");
        else
            Account.instance.armory.BuyItemSlots(1, -10);
    }

    public void GetMoney()
    {
        Account.instance.ChangeMoney(10);
    }

    public void LoadScene(string name)
    {
        LoadingScreen.TransitionTo(name, null);
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
