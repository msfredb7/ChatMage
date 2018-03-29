using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicEnder : MonoBehaviour
{
    public class OnCompletion
    {
        public string SceneName;
        public SceneMessage SceneMessage;
        public bool WaitForNextSceneToSetup;
        public OnCompletion(string sceneName, SceneMessage sceneMessage, bool waitForNextSceneToSetup)
        {
            SceneName = sceneName;
            SceneMessage = sceneMessage;
            WaitForNextSceneToSetup = waitForNextSceneToSetup;
        }
    }
    public static OnCompletion onCompletion;

    void OnEnable()
    {
        DefaultAudioSources.StopMusicFaded();
        if(onCompletion != null)
        {
            LoadingScreen.TransitionTo(onCompletion.SceneName, onCompletion.SceneMessage, onCompletion.WaitForNextSceneToSetup, Color.black);
        }
        else
        {
            LoadingScreen.TransitionTo(MainMenu.SCENENAME, null, false, Color.black);
        }
    }
}
