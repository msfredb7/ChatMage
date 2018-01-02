using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Manager;
using UnityEngine.SceneManagement;

public class GameDebugLauncher : MonoBehaviour
{
    [Header("Debug")]
    public string defaultLevelScript;

    void Start()
    {
        if (Scenes.SceneCount() == 1 && Scenes.LoadingSceneCount() <= 0)
        {
            MasterManager.Sync(delegate ()
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
