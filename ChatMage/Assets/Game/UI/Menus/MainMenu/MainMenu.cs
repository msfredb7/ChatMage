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
    public Button quitButton;
    public bool easyVersion = true; // POUR LE MIGS HARD(Accès direct au level select et loadout)
                             //              EASY(Jeu normal avec cinématique et saut au niveau 1-1)

    [Header("First time playing")]
    public Level firstLevel;
    public string carAssetName;

    public AudioClip anthem;

    public void Init()
    {
        MasterManager.Sync(delegate ()
        {
            SoundManager.PlayMusic(anthem);
            playButton.onClick.AddListener(OnClick);
            if (firstLevel != null)
                firstLevel.LoadData();
        });
    }

    void OnClick()
    {
        //Check first level. Si le premier level n'a pas été complété, on fait -> cinematic -> first level
        if (firstLevel != null && !firstLevel.HasBeenCompleted && easyVersion)
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
        CinematicSettings cinematicSettings = new CinematicSettings
        {
            skipOnDoubleTap = false,
            nextSceneName = Framework.SCENENAME,
            nextSceneMessage = gameMessage
        };

        //Launch
        CinematicScene.LaunchCinematic("Cinematic Demo", cinematicSettings);
    }

    void GoToLevelSelect()
    {
        LoadingScreen.TransitionTo(LevelSelect.LevelSelection.SCENENAME, null);
    }
}
