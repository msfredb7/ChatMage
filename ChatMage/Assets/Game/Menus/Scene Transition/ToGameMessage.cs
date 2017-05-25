using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class ToGameMessage : SceneMessage
{
    private LevelScript chosenLevel;
    private LoadoutResult loadoutResult;

    public ToGameMessage(LevelScript chosenLevel, LoadoutResult loadoutResult)
    {
        this.chosenLevel = chosenLevel;
        this.loadoutResult = loadoutResult;
    }
    public void OnLoaded(Scene scene)
    {
        Framework framework = Scenes.FindRootObject<Framework>(scene);
        framework.Init(chosenLevel, loadoutResult);
    }

    public void OnOutroComplete()
    {

    }
}
