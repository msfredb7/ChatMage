using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDebugLauncher : MonoBehaviour
{
    [Header("Debug")]
    public string defaultLevelScript;

    void Start()
    {
        if (Scenes.ActiveSceneCount == 1 && Scenes.LoadingSceneCount <= 0)
        {
            PersistentLoader.LoadIfNotLoaded(delegate ()
            {
                Scenes.Load(Framework.SCENENAME, LoadSceneMode.Additive, DebugInit);
            });
        }
    }

    void DebugInit(Scene scene)
    {
        Framework framework = Scenes.FindRootObject<Framework>(scene);
        framework.Init(defaultLevelScript, null);
    }
}
