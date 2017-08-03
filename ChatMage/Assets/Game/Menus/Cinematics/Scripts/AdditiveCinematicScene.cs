using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using CCC.Manager;
using UnityEngine.SceneManagement;

public class AdditiveCinematicScene : BaseCinematicScene
{
    [Header("Additive Settings")]
    public CanvasGroup fadeGroup;
    public float fadeInDuration = 0.5f;
    public float fadeOutDuration = 0.5f;

    void Awake()
    {
        fadeGroup.alpha = 0;
    }

    protected override void OnEnter()
    {
        print("on enter");
        fadeGroup.DOKill();
        fadeGroup.DOFade(1, fadeInDuration).OnComplete(Play);
    }

    protected override void OnExit()
    {
        fadeGroup.DOKill();
        fadeGroup.DOFade(0, fadeOutDuration).OnComplete(delegate ()
        {
            //Unload scene
            Scenes.UnloadAsync(gameObject.scene.name);
        });
    }

    protected override void OnPlay()
    {

    }

    public static void LaunchCinematic(string sceneName, AdditiveCinematicSettings cinematicSettings)
    {
        Scenes.LoadAsync(sceneName, LoadSceneMode.Additive,
            delegate (Scene scene)
            {
                print("on loaded");
                cinematicSettings.OnLoaded(scene);
                cinematicSettings.OnOutroComplete();
            });
    }
}
