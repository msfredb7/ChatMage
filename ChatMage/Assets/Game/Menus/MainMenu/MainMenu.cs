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
    public bool DEMO = false; // A ENLEVER
    public AudioClip title;

    //[InspectorHeader("Tutorial")]
    //public bool doATutorial = false;
    //[InspectorShowIf("doATutorial")]
    //public Tutorial.BaseTutorial tutorial;

    public void Init()
    {
        MasterManager.Sync(delegate() {
            playButton.onClick.AddListener(GotoLevelSelect);

            SoundManager.PlaySFX(title);

            //if(doATutorial)
            //    StartTutorial();
        });
    }

    public void GotoLevelSelect()
    {
        if(DEMO) // A ENLEVER
            LoadingScreen.TransitionTo(Framework.SCENENAME, null);
        LoadingScreen.TransitionTo(LevelSelect.LevelSelection.SCENENAME, null);
    }

    //void StartTutorial()
    //{
    //    Tutorial.TutorialScene.StartTutorial(tutorial.name);
    //}
}
