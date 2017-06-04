using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using CCC.Manager;

public class LoadoutTab : MonoBehaviour
{

    public enum LoadoutTab_Type
    {
        Car = 0,
        Smash = 1,
        Items = 2,
        NotSet = 3
    }

    public Loadout loadout;
    public Text title;
    public Transform countainer;
    public LoadoutPreview preview;
    public GameObject gridButtonPrefab;
    public Button equipButton;
    public Dropdown sortBy;
    public Text remainingSlots;

    public float startX;
    public float exitX;
    public float introX;
    public float transitionDuration;

    public GameObject panel;

    private LoadoutTab_Type currentType = 0;
    private List<EquipablePreview> currentEquipables;

    public void DisplayStart(List<EquipablePreview> equipables, LoadoutTab_Type type = LoadoutTab_Type.NotSet, bool firstEntry = false)
    {
        ResetPreview();
        switch (type)
        {
            case LoadoutTab_Type.Car:
                if (firstEntry)
                {
                    title.text = "Step 1: Select Your Car";
                    preview.Disable();
                    CreateGrid(equipables);
                    remainingSlots.gameObject.SetActive(false);
                    PanelIntro(null);
                }
                else
                {
                    PanelOutro(delegate ()
                    {
                        title.text = "Step 1: Select Your Car";
                        CreateGrid(equipables);
                        remainingSlots.gameObject.SetActive(false);
                        PanelIntro(null);
                    });
                }
                currentType = LoadoutTab_Type.Car;
                break;
            case LoadoutTab_Type.Smash:
                if (firstEntry)
                {
                    title.text = "Step 2: Select Your Smash";
                    preview.Disable();
                    CreateGrid(equipables);
                    remainingSlots.gameObject.SetActive(false);
                    PanelIntro(null);
                }
                else
                {
                    PanelOutro(delegate ()
                    {
                        title.text = "Step 2: Select Your Smash";
                        CreateGrid(equipables);
                        remainingSlots.gameObject.SetActive(false);
                        PanelIntro(null);
                    });
                }
                currentType = LoadoutTab_Type.Smash;
                break;
            case LoadoutTab_Type.Items:
                if (firstEntry)
                {
                    title.text = "Final Step: Select Your Items";
                    preview.Disable();
                    CreateGrid(equipables);
                    remainingSlots.gameObject.SetActive(true);
                    remainingSlots.text = "Slots : " + (loadout.currentLoadout.itemSlotAmount - loadout.currentLoadout.itemOrders.Count);
                    PanelIntro(null);
                }
                else
                {
                    PanelOutro(delegate ()
                    {
                        title.text = "Final Step: Select Your Items";
                        CreateGrid(equipables);
                        remainingSlots.gameObject.SetActive(true);
                        remainingSlots.text = "Slots : " + (loadout.currentLoadout.itemSlotAmount - loadout.currentLoadout.itemOrders.Count);
                        PanelIntro(null);
                    });
                }
                currentType = LoadoutTab_Type.Items;
                break;
            case LoadoutTab_Type.NotSet:
                DisplayStart(equipables, currentType);
                break;
        }
    }

    public void DisplayAll(LoadoutTab_Type type)
    {
        switch (type)
        {
            case LoadoutTab_Type.Car:
                currentType = LoadoutTab_Type.Car;
                preview.Disable();
                CreateGrid(loadout.armory.cars);
                break;
            case LoadoutTab_Type.Smash:
                currentType = LoadoutTab_Type.Smash;
                preview.Disable();
                CreateGrid(loadout.armory.smashes);
                break;
            case LoadoutTab_Type.Items:
                currentType = LoadoutTab_Type.Items;
                preview.Disable();
                CreateGrid(loadout.armory.items);
                break;
        }
    }

    public void DisplayUnlock(LoadoutTab_Type type)
    {
        switch (type)
        {
            case LoadoutTab_Type.Car:
                currentType = LoadoutTab_Type.Car;
                preview.Disable();
                CreateGrid(loadout.armory.GetAllUnlockedCars());
                break;
            case LoadoutTab_Type.Smash:
                currentType = LoadoutTab_Type.Smash;
                preview.Disable();
                CreateGrid(loadout.armory.GetAllUnlockedSmash());
                break;
            case LoadoutTab_Type.Items:
                currentType = LoadoutTab_Type.Items;
                preview.Disable();
                CreateGrid(loadout.armory.GetAllUnlockedItems());
                break;
        }
    }

    public void DisplayLock(LoadoutTab_Type type)
    {
        switch (type)
        {
            case LoadoutTab_Type.Car:
                currentType = LoadoutTab_Type.Car;
                preview.Disable();
                CreateGrid(loadout.armory.GetAllLockedCars());
                break;
            case LoadoutTab_Type.Smash:
                currentType = LoadoutTab_Type.Smash;
                preview.Disable();
                CreateGrid(loadout.armory.GetAllLockedSmash());
                break;
            case LoadoutTab_Type.Items:
                currentType = LoadoutTab_Type.Items;
                preview.Disable();
                CreateGrid(loadout.armory.GetAllLockedItems());
                break;
        }
    }

    private void CreateGrid(List<EquipablePreview> equipables)
    {
        Clear();
        currentEquipables = equipables;
        for (int i = 0; i < equipables.Count; i++)
        {
            // Equipable Preview
            EquipablePreview newPreview = equipables[i];

            // Ajout du Boutton
            GameObject newButton = Instantiate(gridButtonPrefab, countainer);

            // Apparence (unlock/equip)
            if(!newPreview.unlocked)
                newButton.GetComponent<Image>().color = Color.gray;
            if(loadout.currentLoadout.AlreadyEquip(newPreview.equipableAssetName,newPreview.type))
                newButton.GetComponent<Image>().color = Color.red;
            newButton.GetComponent<Image>().sprite = newPreview.icon;
            newButton.GetComponentInChildren<Text>().text = newPreview.displayName;

            // Gestion des evennements
            newButton.GetComponent<Button>().onClick.AddListener(delegate ()
            {
                // Transfert des variables locales
                EquipablePreview currentPreview = newPreview;
                GameObject currentButton = newButton;

                // Display du EquipablePreview
                preview.DisplayPreview(currentPreview);

                // Ajustement au niveau du equipButton
                if (!equipButton.gameObject.activeSelf)
                    equipButton.gameObject.SetActive(true);
                equipButton.onClick.RemoveAllListeners();

                // Equip Button est un equip ou shop
                if (!currentPreview.unlocked)
                {
                    equipButton.image.color = Color.yellow;
                    equipButton.GetComponentInChildren<Text>().text = "SHOP";
                    equipButton.onClick.AddListener(GoToShop);
                }
                else
                {
                    equipButton.image.color = Color.white;
                    equipButton.GetComponentInChildren<Text>().text = "EQUIP";

                    // Gestion de l'evennement de lorsque l'objet est equiper
                    equipButton.onClick.AddListener(delegate ()
                    {
                        if(currentPreview.type != EquipableType.Item)
                            ClearFocus();
                        if (loadout.Equip(currentPreview))
                        {
                            currentButton.GetComponent<Image>().color = Color.red;  // A changer
                            loadout.currentLoadout.Save();
                            remainingSlots.text = "Slots : " + (loadout.currentLoadout.itemSlotAmount - loadout.currentLoadout.itemOrders.Count);
                        }
                    });
                }
            });
        }
    }

    private void GoToShop()
    {
        LoadingScreen.TransitionTo(ShopMenu.SCENENAME, new ToShopMessage(Loadout.SCENENAME,currentType));
    }

    public void Clear()
    {
        foreach (Transform child in countainer)
        {
            Destroy(child.gameObject);
        }
    }

    public void OnSortChange(Dropdown target)
    {
        switch (target.value)
        {
            case 0:
                DisplayAll(currentType);
                break;
            case 1:
                DisplayUnlock(currentType);
                break;
            case 2:
                DisplayLock(currentType);
                break;
        }
    }

    public void ResetPreview()
    {
        equipButton.gameObject.SetActive(false);
        preview.Disable();
    }

    public void PanelOutro(TweenCallback callback)
    {
        loadout.backButton.interactable = false;
        loadout.nextButton.interactable = false;
        title.DOFade(0, transitionDuration);
        Tween animation = panel.gameObject.GetComponent<RectTransform>().DOAnchorPosX(exitX, transitionDuration);
        animation.OnComplete(callback);
    }

    public void PanelIntro(TweenCallback callback)
    {
        if (!title.gameObject.activeSelf)
            title.gameObject.SetActive(true);
        if (!panel.gameObject.activeSelf)
            panel.gameObject.SetActive(true);
        title.DOFade(1, transitionDuration);
        panel.gameObject.GetComponent<RectTransform>().position = new Vector2(introX, panel.GetComponent<RectTransform>().position.y);
        Tween animation = panel.gameObject.GetComponent<RectTransform>().DOAnchorPosX(startX, transitionDuration);
        loadout.backButton.interactable = true;
        loadout.nextButton.interactable = true;
        animation.OnComplete(callback);
    }

    private void ClearFocus()
    {
        foreach (Transform child in countainer.transform)
        {
            // A changer
            if(child.GetComponent<Image>().color != Color.gray)
                child.GetComponent<Image>().color = Color.white; 
        }
    }
}
