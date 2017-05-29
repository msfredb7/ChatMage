using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public const string SCENENAME = "MainMenu";
    public Button shopButton;
    public Button playButton;
    public Button quitButton;

    void Start()
    {
        MasterManager.Sync();

        shopButton.onClick.AddListener(GotoShop);
        quitButton.onClick.AddListener(Quit);
        playButton.onClick.AddListener(GotoLevelSelect);
    }

    private void GotoShop()
    {
        //TODO utiliser 'SCENENAME'
        LoadingScreen.TransitionTo("Shop", null);
    }

    private void GotoLevelSelect()
    {
        LoadingScreen.TransitionTo(LevelSelection.SCENENAME, null);
    }

    private void Quit()
    {
        Application.Quit();
    }
}
