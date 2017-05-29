using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public const string SCENENAME = "LevelSelect";
    private const string LASTLEVELSELECTED_KEY = "lls";
    public List<LevelSelect_Region> regions;
    public Button backButton;

    void Start()
    {
        MasterManager.Sync(OnSync);
        backButton.onClick.AddListener(OnBackClicked);
    }

    void OnSync()
    {
        print("sync complete");
        AddListeners();


        // Should we mark a level as 'completed' ?
        bool lastGameResult = GetLastGameResult();

        if (lastGameResult)
        {
            string lastLevelSelected = GameSaves.instance.GetString(GameSaves.Type.LevelSelect, LASTLEVELSELECTED_KEY);
            SetAsCompleted(lastLevelSelected);
        }

        //Un peu lourd ? Peut-être qu'on pourrait faire ça AVANT que le loading screen disparaisse (comme Framework)
        LoadAllData();
    }

    bool GetLastGameResult()
    {
        if (GameSaves.instance.ContainsBool(GameSaves.Type.LevelSelect, LevelScript.WINRESULT_KEY))
        {
            return GameSaves.instance.GetBool(GameSaves.Type.LevelSelect, LevelScript.WINRESULT_KEY);
        }
        return false;
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

        GameSaves.instance.SetString(GameSaves.Type.LevelSelect, LASTLEVELSELECTED_KEY, level.name);

        ToLoadoutMessage message = new ToLoadoutMessage(level.levelScriptName);
        LoadingScreen.TransitionTo(Loadout.SCENENAME, message);
    }

    void OnBackClicked()
    {
        LoadingScreen.TransitionTo(MainMenu.SCENENAME, null);
    }
    
    private void SetAsCompleted(string levelName)
    {
        for (int i = 0; i < regions.Count; i++)
        {
            if (regions[i].SetAsCompleted(levelName))
                return;
        }
    }

}
