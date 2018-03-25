using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdsRelayer : SceneMessage
{
    public const string SCENENAME = "Ad";

    public SceneMessage message;
    public string previousSceneName;
    public string nextSceneName;

    public AdsRelayer(SceneMessage message, string previousSceneName, string nextSceneName)
    {
        this.message = message;
        this.previousSceneName = previousSceneName;
        this.nextSceneName = nextSceneName;
    }

    public void OnLoaded(Scene scene)
    {
#if UNITY_ADS
        /*
        GameObject[] objets = scene.GetRootGameObjects();
        for (int i = 0; i < objets.Length; i++)
        {
            if(objets[i].GetComponent<AdsStarter>() != null)
            {
                objets[i].GetComponent<AdsStarter>().ShowRewardedAd(this);
                return;
            }
        }
        */
#endif
    }

    public void OnOutroComplete()
    {

    }
}
