using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class PlayLevelMessage : SceneMessage
{
    private LevelScript chosenLevel;
    public PlayLevelMessage(LevelScript chosenLevel)
    {
        this.chosenLevel = chosenLevel;
    }
    public void OnLoaded(Scene scene)
    {
        Framework framework = Scenes.FindRootObject<Framework>(scene);
        framework.Init(chosenLevel);
    }

    public void OnOutroComplete()
    {
        //Start la game
    }
}
