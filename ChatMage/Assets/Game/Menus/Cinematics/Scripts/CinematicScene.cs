using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CinematicScene : BaseCinematicScene
{
    [Header("CinematicScene Settings")]
    public Camera localCamera;

    private string nextSceneName;
    private SceneMessage nextSceneMessage;
    private bool waitForNextSceneSetup;

    void Awake()
    {
        localCamera.enabled = true;
    }

    public override void ApplySettings<T>(T settings)
    {
        base.ApplySettings(settings);

        if(settings is CinematicSettings)
        {
            CinematicSettings advSettings = settings as CinematicSettings;
            nextSceneMessage = advSettings.nextSceneMessage;
            nextSceneName = advSettings.nextSceneName;
            waitForNextSceneSetup = advSettings.waitForNextSceneSetup;
        }
        else
        {
            Debug.LogWarning("Cinematic settings applied to 'CinematicScene' should be of type 'CinematicSettings'.");
        }
    }

    public override void OnArrivalComplete()
    {
        base.OnArrivalComplete();

        Play();
    }

    protected override void OnEnter()
    {
        //Do nothing
    }

    protected override void OnExit()
    {
        //Do nothing
        LoadingScreen.TransitionTo(nextSceneName, nextSceneMessage, waitForNextSceneSetup);
    }

    protected override void OnPlay()
    {
        //Do nothing
    }

    public static void LaunchCinematic(string sceneName, CinematicSettings cinematicSettings)
    {
        LoadingScreen.TransitionTo(sceneName, cinematicSettings);
    }
}