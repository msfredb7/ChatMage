using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public const string SCENENAME = "LevelSelect";
    public List<LevelSelect_Region> regions;
    public Button backButton;

    void Start()
    {
        MasterManager.Sync(OnSync);
        backButton.onClick.AddListener(OnBackClicked);
    }

    void OnSync()
    {
        AddListeners();

        //Un peu lourd ? Peut-être qu'on pourrait faire ça AVANT que le loading screen disparaisse (comme Framework)
        LoadAllData();
    }

    void AddListeners()
    {
        for (int i = 0; i < regions.Count; i++)
        {
            regions[i].onLevelSelected += OnLevelSelected;
        }
    }

    void LoadAllData()
    {
        for (int i = 0; i < regions.Count; i++)
        {
            regions[i].LoadData();
        }
    }

    void OnLevelSelected(Level level)
    {
        //Go to loadout !
        print("Level selected: " + level.name);

        ToLoadoutMessage message = new ToLoadoutMessage(level.levelScriptName);
        LoadingScreen.TransitionTo(Loadout.SCENENAME, message);
    }

    void OnBackClicked()
    {
        LoadingScreen.TransitionTo(MainMenu.SCENENAME, null);
    }

}
