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
        Scenes.Load(Framework.SCENENAME, LoadSceneMode.Additive, OnLoadComplete);
    }

    void OnLoadComplete(Scene scene)
    {
        Scenes.UnloadAsync("MenuSelection");

        Framework framework = Scenes.FindRootObject<Framework>(scene);
        framework.Init(level);
        isLoading = false;
    }
}
