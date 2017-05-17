using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSelection : MonoBehaviour
{
    public static bool isLoading = false;
    public LevelScript level;

    public void LoadLevel()
    {
        if (isLoading)
            return;

        isLoading = true;

        PlayLevelMessage message = new PlayLevelMessage(level);
        LoadingScreen.TransitionTo(Framework.SCENENAME, message);
    }

    void OnDestroy()
    {
        isLoading = false;
    }
}
