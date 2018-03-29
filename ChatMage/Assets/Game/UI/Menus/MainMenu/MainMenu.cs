
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
    public bool easyVersion = true; // POUR LE MIGS HARD(Accès direct au level select et loadout)
                                    //              EASY(Jeu normal avec cinématique et saut au niveau 1-1)
    public bool debugEndless = false;

    public Button infoCredits;
    public SceneInfo credits;

    private List<Button> modeButtons;
    private int currentPosition;
    private Vector3 leftPos;
    private Vector3 rightPos;
    private Vector3 midPos;
    public Button adventureButton;
    public Button endlessButton;
    public Button testButton;
    public Button goRight;
    public Button goLeft;
    public float animDuration = 1.0f;

    [Header("First time playing")]
    public SceneInfo cinematicScene;
    public Level firstLevel;
    public SceneInfo stageSelectionInfo;
    public string carAssetName;
    public LS_ThridLevel thirdLevel;

    [Header("Music")]
    public AudioAsset music;


    public void Init()
    {
        PersistentLoader.LoadIfNotLoaded(delegate ()
        {
            DefaultAudioSources.PlayMusic(music);

            adventureButton.onClick.AddListener(OnAdventureClick);
            endlessButton.onClick.AddListener(OnEndlessClick);
            goRight.onClick.AddListener(ModesGoRight);
            goLeft.onClick.AddListener(ModesGoLeft);
            infoCredits.onClick.AddListener(OpenCredits);
            if (firstLevel != null)
                firstLevel.LoadData();
        });

        MakeButtonList();

        // Verify if Endless Mode is accessible
        if (debugEndless)
        {
            SetTextButtonActive(endlessButton, true);
            SetTextButtonActive(goLeft, true);
        }
        else
        {
            if (thirdLevel.dataSaver.ContainsBool(LevelScript.COMPLETED_KEY + thirdLevel.name))
            {
                if (!thirdLevel.dataSaver.GetBool(LevelScript.COMPLETED_KEY + thirdLevel.name))
                {
                    SetTextButtonActive(endlessButton, false);
                    SetTextButtonActive(goLeft, false);
                }
                else
                {
                    SetTextButtonActive(endlessButton, true);
                    SetTextButtonActive(goLeft, true);
                }
            }
            else
            {
                SetTextButtonActive(endlessButton, false);
                SetTextButtonActive(goLeft, false);
            }
        }

        SetTextButtonActive(goRight, false);
    }

    void OpenCredits()
    {
        Scenes.Load(credits);
    }

    void MakeButtonList()
    {
        modeButtons = new List<Button>();
        modeButtons.Add(testButton);
        modeButtons.Add(adventureButton);
        modeButtons.Add(endlessButton);

        currentPosition = 1;

        midPos = adventureButton.transform.position;
        leftPos = testButton.transform.position;
        rightPos = endlessButton.transform.position;
    }

    void OnAdventureClick()
    {
        //Check first level. Si le premier level n'a pas été complété, on fait -> cinematic -> first level
        if (firstLevel != null && !firstLevel.HasBeenCompleted && easyVersion)
            GoToFirstLevel();
        else
            GoToLevelSelect();
    }

    void OnEndlessClick()
    {
        LoadingScreen.TransitionTo(stageSelectionInfo.SceneName, null);
    }

    void GoToFirstLevel()
    {
        //Loadout
        LoadoutResult loadoutResult = new LoadoutResult();
        loadoutResult.AddEquipable(carAssetName, EquipableType.Car);

        //Scene message à donner au framework
        ToGameMessage gameMessage = new ToGameMessage(firstLevel.levelScriptName, loadoutResult, true);

        //Cinematic Settings
        //CinematicSettings cinematicSettings = new CinematicSettings
        //{
        //    skipOnDoubleTap = false,
        //    nextSceneName = Framework.SCENENAME,
        //    nextSceneMessage = gameMessage
        //};

        ////Launch
        //CinematicScene.LaunchCinematic("Cinematic Demo", cinematicSettings);

        // ON NE PASSE PAS PAR L'INTRO
        CinematicEnder.onCompletion = new CinematicEnder.OnCompletion(Framework.SCENENAME, gameMessage, true);
        DefaultAudioSources.StopMusicFaded();
        LoadingScreen.TransitionTo(cinematicScene.SceneName, null, false, Color.black);
        //LoadingScreen.TransitionTo(Framework.SCENENAME, gameMessage, true);
    }

    void GoToLevelSelect()
    {
        LoadingScreen.TransitionTo(LevelSelect.LevelSelection.SCENENAME, null);
    }

    void SetTextButtonActive(Button button, bool active)
    {
        button.interactable = active;

        if (button.GetComponent<FadeFlash>() == null)
            return;

        Text text = button.GetComponentInChildren<Text>();
        if (text != null)
        {
            if (active)
                text.color = text.color.ChangedAlpha(1);
            else
                text.color = text.color.ChangedAlpha(0.5f);
        }

        FadeFlash flash = button.GetComponent<FadeFlash>();
        if (flash != null)
        {
            if (!active)
            {
                flash.Stop();
                button.GetComponent<CanvasGroup>().alpha = 0.5f;
            }
        }
    }
    
    void ModesGoLeft()
    {
        // Do animation
        modeButtons[currentPosition].gameObject.transform.DOMove(leftPos,animDuration);
        modeButtons[currentPosition+1].gameObject.transform.DOMove(midPos, animDuration);

        currentPosition++;

        if ((currentPosition + 1) > (modeButtons.Count - 1))
            SetTextButtonActive(goLeft, false);
        else
            SetTextButtonActive(goLeft, true);

        SetTextButtonActive(goRight, true);
    }

    void ModesGoRight()
    {
        // Do animation
        modeButtons[currentPosition].gameObject.transform.DOMove(rightPos, animDuration);
        modeButtons[currentPosition - 1].gameObject.transform.DOMove(midPos, animDuration);

        currentPosition--;

        if ((currentPosition - 1) < (modeButtons.Count - 1))
            SetTextButtonActive(goRight, false);
        else
            SetTextButtonActive(goRight, true);

        SetTextButtonActive(goLeft, true);
    }
}
