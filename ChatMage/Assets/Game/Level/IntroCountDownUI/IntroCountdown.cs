using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroCountdown : MonoBehaviour {

    public LevelScript defaultLevelScript;

    void Start()
    {
        if (Scenes.SceneCount() == 1)
        {
            MasterManager.Sync(delegate ()
            {
                Scenes.Load("Framework", LoadSceneMode.Additive, DebugInit);
            });
        }
        Init();
    }

    void DebugInit(Scene scene)
    {
        Framework framework = Scenes.FindRootObject<Framework>(scene);
        framework.Init(defaultLevelScript, null);
        Init();
    }

    void Init()
    {
        DelayManager.CallTo(delegate () { GetComponent<Text>().text = "3"; }, 0);
        DelayManager.CallTo(delegate () { GetComponent<Text>().text = "2"; }, 1);
        DelayManager.CallTo(delegate () { GetComponent<Text>().text = "1"; }, 2);
        DelayManager.CallTo(delegate () { Scenes.UnloadAsync("IntroCountDown"); }, 3);
    }
}
