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

    [InspectorHeader("Tutorial")]
    public bool doATutorial = false;
    [InspectorShowIf("doATutorial")]
    public BaseTutorial tutorial;

    void Start()
    {
        MasterManager.Sync();

        playButton.onClick.AddListener(GotoLevelSelect);

        if(doATutorial)
            StartTutorial();
    }

    private void GotoLevelSelect()
    {
        LoadingScreen.TransitionTo(LevelSelect.LevelSelection.SCENENAME, null);
    }

    void StartTutorial()
    {
        Scenes.LoadAsync("Tutorial", LoadSceneMode.Additive, delegate (Scene scene) {
            GameObject[] obj = scene.GetRootGameObjects();
            for (int i = 0; i < obj.Length; i++)
            {
                TutorialStarter starter = obj[i].GetComponent<TutorialStarter>();
                if (starter != null)
                {
                    Debug.Log("Init the tutorial");
                    starter.Init(tutorial);
                    break;
                }
            }
        });
    }
}
