using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen
{
    private class Wish
    {
        public SceneMessage message;
        public string sceneName;
    }
    private const string SCENENAME = "LoadingScreen";
    private static Wish wish;
    private static bool isInTransition = false;
    private static LoadingScreenAnimation animator;

    public static void TransitionTo(string sceneName, SceneMessage message)
    {
        if (isInTransition)
        {
            Debug.LogWarning("Cannot transition to " + sceneName + ". We are already transitioning.");
            return;
        }

        isInTransition = true;

        wish = new Wish();
        wish.message = message;
        wish.sceneName = sceneName;

        Scenes.LoadAsync(SCENENAME, LoadSceneMode.Additive, OnLoadingScreenLoaded);
    }

    private static void OnLoadingScreenLoaded(Scene scene)
    {
        animator = Scenes.FindRootObject<LoadingScreenAnimation>(scene);
        animator.Intro(OnIntroComplete);
    }

    private static void OnIntroComplete()
    {
        //Unload all past scenes
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name != SCENENAME)
                Scenes.UnloadAsync(SceneManager.GetSceneAt(i).name);
        }

        Scenes.LoadAsync(wish.sceneName, LoadSceneMode.Additive, OnDestinationLoaded);
    }

    private static void OnDestinationLoaded(Scene scene)
    {
        animator.Outro(OnOutroComplete);
        if (wish.message != null)
            wish.message.OnLoaded(scene);
    }

    private static void OnOutroComplete()
    {
        Scenes.UnloadAsync(SCENENAME);
        if (wish.message != null)
            wish.message.OnOutroComplete();

        wish = null;
        animator = null;
        isInTransition = false;
    }
}
