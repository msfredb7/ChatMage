using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class ShopMenu : MonoBehaviour
{
    
    void Start()
    {
        MasterManager.Sync(OnSync);
    }

    void OnSync()
    {
        if (Account.instance == null)
            Scenes.Load("MainMenu");
    }

    public void BuyLootBox()
    {
        new LootBox(Account.instance.armory, LootBox.LootBoxType.common);
    }

    public void BuySlots()
    {
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
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
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
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }
}
