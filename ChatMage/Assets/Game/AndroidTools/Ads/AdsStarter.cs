using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ADS
using UnityEngine.Advertisements;
#endif

public class AdsStarter : MonoBehaviour
{

    private SceneMessage currentMessage;
    private string previousSceneName;
    private string nextSceneName;

    private Action onAdComplete;
    public bool IsConnected { get; private set; }

    private const string key = "previouslyDoneAd";

    // ADS
    IEnumerator CheckInternetConnection(Action<bool> action)
    {
        yield return null;
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("No internet connection");
            action(false);
        }
        else
        {
            action(true);
        }
        //WWW www = new WWW("http://google.com");
        //yield return www;
        //if (www.error != null)
        //{
        //    action(false);
        //}
        //else
        //{
        //    action(true);
        //}
    }
    void Start()
    {
        IsConnected = false;
        StartCoroutine(CheckInternetConnection((isConnected) =>
        {
            IsConnected = isConnected;
        }));
    }

    public void ShowRewardedAd(/*AdsRelayer relayerMessage,*/ Action onComplete = null)
    {
#if UNITY_ADS
        bool doAd = (PlayerPrefs.GetInt(key, 0) == 0 ? true : false);
        if (doAd)
        {
            PlayerPrefs.SetInt(key, 1);
            //this.DelayedCall(delegate () {
            if (Advertisement.IsReady("video"))
            {
                var options = new ShowOptions { resultCallback = HandleShowResult };
                Advertisement.Show("video", options);
                onAdComplete = onComplete;
                return;
            }
            else
            {
                PopUpMenu.ShowOKPopUpMenu("Could not connect", "You are not connected to the internet. Please verify your connection.", delegate ()
                {
                    //LoadingScreen.TransitionTo(previousSceneName, null);
                    if (onComplete != null)
                        onComplete();
                });
            }
        }
        else
        {
            PlayerPrefs.SetInt(key, 0);
        }
#endif
    }

#if UNITY_ADS
    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                //Debug.Log("The ad was successfully shown.");
                //LoadingScreen.TransitionTo(nextSceneName, currentMessage, true);
                if (onAdComplete != null)
                    onAdComplete();
                return;
            case ShowResult.Skipped:
                //Debug.Log("The ad was skipped before reaching the end.");
                //LoadingScreen.TransitionTo(nextSceneName, currentMessage, true);
                if (onAdComplete != null)
                    onAdComplete();
                return;
            case ShowResult.Failed:
                //Debug.LogError("The ad failed to be shown.");
                //LoadingScreen.TransitionTo(previousSceneName, null);
                if (onAdComplete != null)
                    onAdComplete();
                return;
        }
        // Si le callback est fait, mais que le result est bizarre, on continue
        //LoadingScreen.TransitionTo(nextSceneName, currentMessage, true);
        if (onAdComplete != null)
            onAdComplete();
    }
#endif

}
