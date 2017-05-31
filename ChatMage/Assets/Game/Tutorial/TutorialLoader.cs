using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialLoader {

    public static void Load(BaseTutorial tutorial)
    {
        Scenes.LoadAsync("Tutorial", LoadSceneMode.Additive, delegate (Scene scene)
        {
            Scenes.FindRootObject<TutorialStarter>(scene).Init(tutorial);
        });
    }
}
