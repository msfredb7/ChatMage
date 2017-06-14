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

    [Header("Linking")]
    public GameObject deactivateScenePanel;
    public Armory armory;

    public Button smallLootbox;
    public Button mediumLootbox;
    public Button largeLootbox;

    private string previousScene;
    private LoadoutMenu.LoadoutTab previousTab;
    private string levelScriptName;

    void Awake()
    {
        if (Scenes.SceneCount() == 1)
        {
            //Debug launch !
            SetPreviousContext(LevelSelect.LevelSelection.SCENENAME);
        }
    }

    void Start()
    {
        MasterManager.Sync(OnSync);
        deactivateScenePanel.SetActive(false);
    }

    void OnSync()
    {
        if (Account.instance == null)
            throw new System.Exception("'Account' manager has no instance");
        smallLootbox.onClick.AddListener(delegate () { BuyLootBox("small"); });//BuyLootBox(LootBox.LootBoxType.small); });
        mediumLootbox.onClick.AddListener(delegate () { BuyLootBox("medium"); });//BuyLootBox(LootBox.LootBoxType.medium); });
        largeLootbox.onClick.AddListener(delegate () { BuyLootBox("large"); });//BuyLootBox(LootBox.LootBoxType.large); });
    }

    /// <summary>
    /// Ancienne fonction qui achete un lootbox en fonction du type voulu
    /// </summary>
    public void BuyLootBox(LootBox.LootBoxType type)
    {
        PopUpMenu.ShowConfirmPopUpMenu("Bill Confirmation", "You are currently in the process of buying a lootbox. Are you sure you want to buy a " + type + " lootbox ?", delegate ()
       {
           // Animation apparition Lootbox
           switch (type)
           {
               case LootBox.LootBoxType.small:
                   new LootBox(armory, LootBox.LootBoxType.small, delegate (List<EquipablePreview> rewards)
                   {
                       // Disparition du Lootbox
                       for (int i = 0; i < rewards.Count; i++)
                       {
                           // Afficher recompense ?
                           //rewards[i]
                       }
                   });
                   break;
               case LootBox.LootBoxType.medium:
                   new LootBox(armory, LootBox.LootBoxType.medium, delegate (List<EquipablePreview> rewards)
                   {
                       // Disparition du Lootbox
                       for (int i = 0; i < rewards.Count; i++)
                       {
                           // Afficher recompense ?
                           //rewards[i]
                       }
                   });
                   break;
               case LootBox.LootBoxType.large:
                   new LootBox(armory, LootBox.LootBoxType.large, delegate (List<EquipablePreview> rewards)
                   {
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

    /// <summary>
    /// Nouvelle fonction plus ou moins utile dans le shop puisqu'elle utilise des LootboxRef
    /// </summary>
    public void BuyLootBox(string identifiant)
    {
        ResourceLoader.LoadLootBoxRefAsync(identifiant, delegate (LootBoxRef lootbox)
        {
            ShopPopUpMenu.ShowShopPopUpMenu("Bill Confirmation", "You are currently in the process of buying a " + lootbox.identifiant
                + " lootbox. Are you sure you want to buy it?", lootbox.icon, "$" + StorePrice.GetPrice(lootbox.commandType), lootbox.amount, delegate ()
            {
                if (Account.instance.Command(lootbox.commandType))
                {
                    new LootBox(identifiant, delegate (List<EquipablePreview> rewards)
                    {
                        // Code pour l'animation du lootbox
                        ResourceLoader.LoadUIAsync("Lootbox", delegate (GameObject lootboxAnim)
                        {
                            Debug.Log(rewards.Count);
                            GameObject newLootboxAnimation = Instantiate(lootboxAnim, transform);
                            newLootboxAnimation.GetComponent<LootboxAnimation>().AddRewards(rewards);
                        });
                    });
                }
            });
        });
    }

    public void GetMoney(int money)
    {
        Account.instance.AddCoins(money);
    }

    public void BuyMoney(int amount)
    {
        Account.instance.BuyCoins(amount);
    }

    public void BackButton()
    {
        if (previousScene == LoadoutMenu.LoadoutUI.SCENENAME)
            LoadingScreen.TransitionTo(previousScene, new ToLoadoutMessage(levelScriptName, previousTab));
        else
            LoadingScreen.TransitionTo(previousScene, null);
    }

    public void ShowRewardedAd()
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            deactivateScenePanel.SetActive(true);
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
        }
        else
        {
            PopUpMenu.ShowOKPopUpMenu("Could not connect", "You are not connected to the internet. Please verify your connection and come back again.");
        }
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                Account.instance.Command(StorePrice.CommandType.adReward);
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                Account.instance.Command(StorePrice.CommandType.adReward); // TODO : A changer
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                PopUpMenu.ShowOKPopUpMenu("Oups", "A problem has occured and the ad could not be shown.");
                break;
        }
        deactivateScenePanel.SetActive(false);
    }

    public void SetPreviousContext(string previousScene, LoadoutMenu.LoadoutTab previousTab = LoadoutMenu.LoadoutTab.Car, string levelScriptName = "")
    {
        this.previousScene = previousScene;
        this.previousTab = previousTab;
        this.levelScriptName = levelScriptName;
    }
}
