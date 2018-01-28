
using CompleteProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ADS
using UnityEngine.Advertisements;
#endif
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
        if (Scenes.ActiveSceneCount == 1)
        {
            //Debug launch !
            SetPreviousContext(LevelSelect.LevelSelection.SCENENAME);
        }
    }

    void Start()
    {
        PersistentLoader.LoadIfNotLoaded(OnSync);
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
    /// Nouvelle fonction qui utilise des lootboxRef pour ouvrir des lootbox
    /// </summary>
    public void BuyLootBox(string identifiant)
    {
        // On load la lootboxRef
        ResourceLoader.LoadLootBoxRefAsync(identifiant, delegate (LootBoxRef lootbox)
        {
            // On fait un popup pour etre sur que le joueur veut acheter une lootbox
            ShopPopUpMenu.ShowShopPopUpMenu("Bill Confirmation", "You are currently in the process of buying a " + lootbox.identifiant
                + " lootbox. Are you sure you want to buy it?", null, "$" + StorePrice.GetPrice(lootbox.commandType), lootbox.itemsAmount, delegate ()
            {
                // On fait une commande pour acheter la lootbox
                if (Account.instance.Command(lootbox.commandType))
                {
                    // DEBUT DE L'ANIMATION DU LOOTBOX (A changer pour la pinata)
                    ResourceLoader.LoadUIAsync("Lootbox", delegate (GameObject lootboxAnim)
                    {
                        GameObject newLootboxAnimation = Instantiate(lootboxAnim, transform);
                        // On doit savoir si on goldify ou non
                        newLootboxAnimation.GetComponent<LootboxAnimation>().goldifyEvent += delegate ()
                        {
                            bool goldifyWorks = true;
                            // Si on goldify, mais que ta deja toute de unlock, ca sert a rien
                            if (armory.GetAllEquipablesLock().Count < 1)
                            {
                                PopUpMenu.ShowOKPopUpMenu("Useless Gold Upgrade", "You can't upgrade your lootbox to gold when " +
                                "you already have everything unlocked!", null);
                                goldifyWorks = false;
                            }

                            if (goldifyWorks)
                            {
                                // Si on goldify et que ca marche, fuck l'event original d'ouverture du lootbox, on en fait un
                                // nouveau qui considere le goldify
                                newLootboxAnimation.GetComponent<LootboxAnimation>().lootboxOpeningEvent = null;
                                newLootboxAnimation.GetComponent<LootboxAnimation>().lootboxOpeningEvent += delegate ()
                                {
                                    //// On ouvre la Lootbox et on obtient les recompenses
                                    //new LootBox(identifiant, delegate (List<LootBoxRewards> rewards)
                                    //{
                                    //    // On ajoute les recompenses dans l'animation
                                    //    newLootboxAnimation.GetComponent<LootboxAnimation>().AddRewards(rewards);
                                    //}, true);
                                };
                            }
                        };
                        // Event de base ou on ouvre un lootbox normal
                        newLootboxAnimation.GetComponent<LootboxAnimation>().lootboxOpeningEvent += delegate ()
                        {
                            // On ouvre la Lootbox et on obtient les recompenses
                            //new LootBox(identifiant, delegate (List<LootBoxRewards> rewards)
                            //{
                            //    // On ajoute les recompenses dans l'animation
                            //    newLootboxAnimation.GetComponent<LootboxAnimation>().AddRewards(rewards);
                            //}, false);
                        };
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
#if UNITY_ANDROID
        Account.instance.BuyCoins(amount);
#endif
    }

    public void BackButton()
    {
        if (previousScene == LoadoutMenu.LoadoutUI.SCENENAME)
            LoadingScreen.TransitionTo(previousScene, new ToLoadoutMessage(levelScriptName, previousTab));
        else
            LoadingScreen.TransitionTo(previousScene, null);
    }

#if UNITY_ADS
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
#endif

    public void SetPreviousContext(string previousScene, LoadoutMenu.LoadoutTab previousTab = LoadoutMenu.LoadoutTab.Car, string levelScriptName = "")
    {
        this.previousScene = previousScene;
        this.previousTab = previousTab;
        this.levelScriptName = levelScriptName;
    }
}
