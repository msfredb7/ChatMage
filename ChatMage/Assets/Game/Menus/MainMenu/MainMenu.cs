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
    public Button optionsMenuButton;
    public bool DEMO = false; // A ENLEVER

    //[InspectorHeader("Tutorial")]
    //public bool doATutorial = false;
    //[InspectorShowIf("doATutorial")]
    //public Tutorial.BaseTutorial tutorial;

    void Start()
    {
        MasterManager.Sync();

        playButton.onClick.AddListener(GotoLevelSelect);

        //if(doATutorial)
        //    StartTutorial();
    }

    private void GotoLevelSelect()
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
