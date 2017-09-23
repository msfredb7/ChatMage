using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using FullInspector;

public class MainMenu : BaseBehavior
{
    public const string SCENENAME = "MainMenu";
    public Button playButton;
    public AudioClip title;

    [Header("First time playing")]
    public Level firstLevel;
    public string carAssetName;

    public void Init()
    {
        MasterManager.Sync(delegate ()
        {
            playButton.onClick.AddListener(OnClick);
            SoundManager.PlaySFX(title);
            if (firstLevel != null)
                firstLevel.LoadData();
        });
    }

    void OnClick()
    {
        //Check first level. Si le premier level n'a pas été complété, on fait -> cinematic -> first level
        if (firstLevel != null && !firstLevel.HasBeenCompleted)
        {
            GoToFirstLevel();
        }
        else
        {
            GoToLevelSelect();
        }
    }

    void GoToFirstLevel()
    {
        //Loadout
        LoadoutResult loadoutResult = new LoadoutResult();
        loadoutResult.AddEquipable(carAssetName, EquipableType.Car);

        //Scene message à donner au framework
        ToGameMessage gameMessage = new ToGameMessage(firstLevel.levelScriptName, loadoutResult, false);

        //Cinematic Settings
        CinematicSettings cinematicSettings = new CinematicSettings();
        cinematicSettings.skipOnDoubleTap = false;
        cinematicSettings.nextSceneName = Framework.SCENENAME;
        cinematicSettings.nextSceneMessage = gameMessage;

        //Launch
        CinematicScene.LaunchCinematic("Cinematic Demo", cinematicSettings);
    }

    void GoToLevelSelect()
    {
        LoadingScreen.TransitionTo(LevelSelect.LevelSelection.SCENENAME, null);
    }
}
