using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ADS
using UnityEngine.Advertisements;
#endif
using UnityEngine.SceneManagement;

public class AdsRelayer : SceneMessage
{

    public const string SCENENAME = "Ad";

    private SceneMessage message;
    private string previousSceneName;
    private string nextSceneName;

    public AdsRelayer(SceneMessage message, string previousSceneName, string nextSceneName)
    {
        this.message = message;
        this.previousSceneName = previousSceneName;
        this.nextSceneName = nextSceneName;
    }

    public void OnLoaded(Scene scene)
    {
#if UNITY_ADS
        ShowRewardedAd();
#endif
    }

    public void OnOutroComplete()
    {

    }

    void LoadNextScene()
    {
        LoadingScreen.TransitionTo(nextSceneName, message, true);
    }

    void LoadPreviousScene()
    {
        LoadingScreen.TransitionTo(previousSceneName, null);
    }

    // ADS
#if UNITY_ADS
    private const string androidGameId = "1426499", iosGameId = "1426500";
    private const bool testMode = true;

    //#if UNITY_ADS
    public void ShowRewardedAd()
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
            return;
        }
        PopUpMenu.ShowOKPopUpMenu("Could not connect", "You are not connected to the internet. Please verify your connection.",delegate() {
            LoadPreviousScene();
        });
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                LoadNextScene();
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                LoadNextScene();
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                LoadPreviousScene();
                break;
        }
    }
#endif
}
