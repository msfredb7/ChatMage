using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using FullInspector;
using DG.Tweening;

public class Loadout : BaseBehavior
{
    public Armory armory;

    private string levelScriptName;

    public GameObject buttonPrefab;

    public Button backButton;
    public Button nextButton;
    public float buttonIntroDuration = 4;

    public GameObject itemButtonsCountainer;
    public GameObject smashButtonsCountainer;
    public GameObject carButtonsCountainer;

    [InspectorDisabled()]
    public LoadoutResult currentLoadout;
    public LoadoutTab tab;

    public const string SCENENAME = "LoadoutMenu";

    private LoadoutTab.LoadoutTab_Type currentTab;

    public void Init(string levelScriptName, LoadoutTab.LoadoutTab_Type startTab = LoadoutTab.LoadoutTab_Type.Car)
    {
        this.levelScriptName = levelScriptName;
        armory.Load();
        currentLoadout = new LoadoutResult(armory.GetItemSlots());
        currentLoadout.Load();

        //// ANCIEN SYSTEME ////

        List<EquipablePreview> unlockItems = armory.GetAllUnlockedItems();
        List<EquipablePreview> unlockCars = armory.GetAllUnlockedCars();
        List<EquipablePreview> unlockSmashes = armory.GetAllUnlockedSmash();

        for (int i = 0; i < unlockItems.Count; i++)
        {
            EquipablePreview currentEquipable = unlockItems[i];
            GameObject newButton = Instantiate(buttonPrefab, itemButtonsCountainer.transform);
            newButton.GetComponent<LoadoutButton>().ChangeLoadoutButton(currentEquipable.displayName);
            newButton.GetComponent<Button>().onClick.AddListener(delegate ()
            {
                Equip(currentEquipable);
            });
        }

        for (int i = 0; i < unlockCars.Count; i++)
        {
            EquipablePreview currentEquipable = unlockCars[i];
            GameObject newButton = Instantiate(buttonPrefab, carButtonsCountainer.transform);
            newButton.GetComponent<LoadoutButton>().ChangeLoadoutButton(currentEquipable.displayName);
            newButton.GetComponent<Button>().onClick.AddListener(delegate ()
            {
                Equip(currentEquipable);
            });
        }

        for (int i = 0; i < unlockSmashes.Count; i++)
        {
            EquipablePreview currentEquipable = unlockSmashes[i];
            GameObject newButton = Instantiate(buttonPrefab, smashButtonsCountainer.transform);
            newButton.GetComponent<LoadoutButton>().ChangeLoadoutButton(currentEquipable.displayName);
            newButton.GetComponent<Button>().onClick.AddListener(delegate ()
            {
                Equip(currentEquipable);
            });
        }

        /////////

        // Back Button
        if (!backButton.gameObject.activeSelf)
        {
            backButton.gameObject.SetActive(true);
            backButton.image.color = new Color(backButton.image.color.r, backButton.image.color.g, backButton.image.color.b,0);
            backButton.image.DOFade(1, buttonIntroDuration);
        }
        backButton.onClick.AddListener(Back);

        // Next Nutton
        if (!nextButton.gameObject.activeSelf)
        {
            nextButton.gameObject.SetActive(true);
            nextButton.image.color = new Color(nextButton.image.color.r, nextButton.image.color.g, nextButton.image.color.b, 0);
            nextButton.image.DOFade(1, buttonIntroDuration);
        }
        nextButton.onClick.AddListener(Next);

        // Load First Tab
        currentTab = startTab;
        tab.equipButton.gameObject.SetActive(false);
        tab.DisplayAll(currentTab);
    }

    public void ChargeLoadoutAndGame()
    {
        if (currentLoadout.carOrder == null)
        {
            currentLoadout.AddEquipable(armory.cars[0].equipableAssetName, EquipableType.Car);
            Debug.LogWarning("No car selected -> default car");
        }
        LoadingScreen.TransitionTo(Framework.SCENENAME, new ToGameMessage(levelScriptName, currentLoadout), false);
        currentLoadout.Save();
    }

    public void BackToLevelSelect()
    {
        LoadingScreen.TransitionTo(LevelSelection.SCENENAME, null);
    }

    public bool Equip(EquipablePreview equipable)
    {
        return currentLoadout.AddEquipable(equipable.equipableAssetName, equipable.type);
    }

    public void Back()
    {
        if (currentTab == LoadoutTab.LoadoutTab_Type.Car)
        {
            tab.PanelOutro(delegate ()
            {
                BackToLevelSelect();
            });
        }

        currentTab--;
        switch (currentTab)
        {
            case LoadoutTab.LoadoutTab_Type.Car:
                tab.DisplayStart(armory.cars, currentTab);
                break;
            case LoadoutTab.LoadoutTab_Type.Smash:
                tab.DisplayStart(armory.smashes, currentTab);
                break;
        }
        tab.ResetPreview();
    }

    public void Next()
    {
        if (currentTab == LoadoutTab.LoadoutTab_Type.Items)
        {
            tab.PanelOutro(delegate ()
            {
                ChargeLoadoutAndGame();
            });
        }

        currentTab++;
        switch (currentTab)
        {
            case LoadoutTab.LoadoutTab_Type.Smash:
                tab.DisplayStart(armory.smashes, currentTab);
                break;
            case LoadoutTab.LoadoutTab_Type.Items:
                tab.DisplayStart(armory.items, currentTab);
                break;
        }
        tab.ResetPreview();
    }

    public void BuySlots()
    {
        PopUpMenu.ShowOKPopUpMenu("Are you sure you want to buy an extra slots for items ?", delegate ()
        {
            if ((Account.instance.GetMoney() - 10) < 0)
                PopUpMenu.ShowPopUpMenu("You don't have enough money. Open loot boxes or win levels to gain money. See you later!",2);
            else
                armory.BuyItemSlots(1, -10);
        });
    }
}
