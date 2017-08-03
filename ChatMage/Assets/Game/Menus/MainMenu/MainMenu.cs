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

    public void Init()
    {
        MasterManager.Sync(delegate() {
            playButton.onClick.AddListener(OnClick);
            SoundManager.PlaySFX(title);
        });
    }

    void OnClick()
    {
        //Allons directement au premier niveau ?
        GotoLevelSelect();
    }

    public void GotoLevelSelect()
    {
        LoadingScreen.TransitionTo(LevelSelect.LevelSelection.SCENENAME, null);
    }
}
