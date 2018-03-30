
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using FullInspector;
using DG.Tweening;
using CCC.UI.Animation;

public class MainMenu : MonoBehaviour
{
    public const string SCENENAME = "MainMenu";
    public bool forceEnableEndless;

    [Header("Buttons")]
    public GameObject endlessButton;
    public GameObject adventureButton;
    public GameObject adventureAloneButton;

    [Header("First time playing")]
    public SceneInfo cinematicScene;
    public Level firstLevel;
    public SceneInfo stageSelectionInfo;
    public string carAssetName;
    public LS_ThridLevel thirdLevel;

    [Header("Music")]
    public AudioAsset music;

    void Awake()
    {
        // Default button layout
        endlessButton.SetActive(false);
        adventureButton.SetActive(false);
        adventureAloneButton.SetActive(true);
    }

    void Start()
    {
        PersistentLoader.LoadIfNotLoaded(delegate ()
        {
            DefaultAudioSources.TransitionToMusic(music, startingVolume: 1);

            if (firstLevel != null)
                firstLevel.LoadData();
        });

        // Enable/Disable buttons if endless mode is available
        bool showEndlessMode = forceEnableEndless || thirdLevel.dataSaver.GetBool(LevelScript.COMPLETED_KEY + thirdLevel.name, false);

        endlessButton.SetActive(showEndlessMode);
        adventureButton.SetActive(showEndlessMode);
        adventureAloneButton.SetActive(!showEndlessMode);
    }

    public void LaunchAdventure()
    {
        //Check first level. Si le premier level n'a pas été complété, on fait -> cinematic -> first level
        if (firstLevel != null && !firstLevel.HasBeenCompleted)
            GoToFirstLevel();
        else
            GoToLevelSelect();

        DefaultAudioSources.StopMusicFaded();
    }

    public void LaunchEndless()
    {
        LoadingScreen.TransitionTo(stageSelectionInfo.SceneName, null);
        DefaultAudioSources.StopMusicFaded();
    }

    void GoToFirstLevel()
    {
        // Loadout
        LoadoutResult loadoutResult = new LoadoutResult();
        loadoutResult.AddEquipable(carAssetName, EquipableType.Car);

        // Scene message à donner au framework
        ToGameMessage gameMessage = new ToGameMessage(firstLevel.levelScriptName, loadoutResult, true);

        // Launch cinematic
        CinematicEnder.onCompletion = new CinematicEnder.OnCompletion(Framework.SCENENAME, gameMessage, true);
        DefaultAudioSources.StopMusicFaded();
        LoadingScreen.TransitionTo(cinematicScene.SceneName, null, false, Color.black);
    }

    void GoToLevelSelect()
    {
        LoadingScreen.TransitionTo(LevelSelect.LevelSelection.SCENENAME, null);
    }
}
