using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public const string SCENENAME = "MainMenu";
    public Button playButton;

    void Start()
    {
        MasterManager.Sync();

        playButton.onClick.AddListener(GotoLevelSelect);
    }

    private void GotoLevelSelect()
    {
        LoadingScreen.TransitionTo(LevelSelection.SCENENAME, null);
    }
}
