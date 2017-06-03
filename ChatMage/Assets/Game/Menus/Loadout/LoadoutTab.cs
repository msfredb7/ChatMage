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
                    CreateGrid(equipables);
                    PanelIntro(null);
                }
                else
                {
                    PanelOutro(delegate ()
                    {
                        title.text = "Step 1: Select Your Car";
                        CreateGrid(equipables);
                        PanelIntro(null);
                    });
                }
                break;
            case LoadoutTab_Type.Smash:
                if (firstEntry)
                {
                    title.text = "Step 2: Select Your Smash";
                    CreateGrid(equipables);
                    PanelIntro(null);
                }
                else
                {
                    PanelOutro(delegate ()
                    {
                        title.text = "Step 2: Select Your Smash";
                        CreateGrid(equipables);
                        PanelIntro(null);
                    });
                }
                break;
            case LoadoutTab_Type.Items:
                if (firstEntry)
                {
                    title.text = "Final Step: Select Your Items";
                    CreateGrid(equipables);
                    PanelIntro(null);
                }
                else
                {
                    PanelOutro(delegate ()
                    {
                        title.text = "Final Step: Select Your Items";
                        CreateGrid(equipables);
                        PanelIntro(null);
                    });
                }
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
                CreateGrid(loadout.armory.cars);
                break;
            case LoadoutTab_Type.Smash:
                CreateGrid(loadout.armory.smashes);
                break;
            case LoadoutTab_Type.Items:
                CreateGrid(loadout.armory.items);
                break;
        }
    }

    public void DisplayUnlock(LoadoutTab_Type type)
    {
        switch (type)
        {
            case LoadoutTab_Type.Car:
                CreateGrid(loadout.armory.GetAllUnlockedCars());
                break;
            case LoadoutTab_Type.Smash:
                CreateGrid(loadout.armory.GetAllUnlockedSmash());
                break;
            case LoadoutTab_Type.Items:
                CreateGrid(loadout.armory.GetAllUnlockedItems());
                break;
        }
    }

    public void DisplayLock(LoadoutTab_Type type)
    {
        switch (type)
        {
            case LoadoutTab_Type.Car:
                CreateGrid(loadout.armory.GetAllLockedCars());
                break;
            case LoadoutTab_Type.Smash:
                CreateGrid(loadout.armory.GetAllLockedSmash());
                break;
            case LoadoutTab_Type.Items:
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
            newButton.GetComponent<Image>().sprite = newPreview.icon;
            newButton.GetComponentInChildren<Text>().text = newPreview.displayName;

            // Gestion des evennements
            newButton.GetComponent<Button>().onClick.AddListener(delegate ()
            {
                EquipablePreview currentPreview = newPreview;
                GameObject currentButton = newButton;
                preview.DisplayPreview(currentPreview);
                equipButton.onClick.RemoveAllListeners();
                if (!currentPreview.unlocked)
                {
                    equipButton.image.color = Color.yellow;
                    equipButton.GetComponentInChildren<Text>().text = "SHOP";
                    equipButton.onClick.AddListener(GoToShop);
                }
                else
                {
                    equipButton.onClick.AddListener(delegate ()
                    {
                        if (!equipButton.gameObject.activeSelf)
                            equipButton.gameObject.SetActive(true);
                        ClearFocus();
                        currentButton.GetComponent<Image>().color = Color.red;  // A changer
                        loadout.Equip(currentPreview);
                    });
                }
            });
        }
    }

    private void GoToShop()
    {
        if (!equipButton.gameObject.activeSelf)
            equipButton.gameObject.SetActive(true);
        ClearFocus();
        Scenes.LoadAsync(ShopMenu.SCENENAME, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        //LoadingScreen.TransitionTo(ShopMenu.SCENENAME, null); // A changer pour additif
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
        title.DOFade(0, transitionDuration);
        Tween animation = panel.gameObject.GetComponent<RectTransform>().DOMoveX(exitX, transitionDuration);
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
        Tween animation = panel.gameObject.GetComponent<RectTransform>().DOMoveX(startX, transitionDuration);
        animation.OnComplete(callback);
    }

    private void ClearFocus()
    {
        foreach (Transform child in countainer.transform)
        {
            child.GetComponent<Image>().color = Color.white; // A changer
        }
    }
}
