using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class ToGameMessage : SceneMessage
{
    private string levelScriptName;
    private LoadoutResult loadoutResult;
    private bool canGoToLevelSelect;

    public ToGameMessage(string levelScriptName, LoadoutResult loadoutResult, bool canGoToLevelSelect = true)
    {
        this.levelScriptName = levelScriptName;
        this.loadoutResult = loadoutResult;
        this.canGoToLevelSelect = canGoToLevelSelect;
    }
    public void OnLoaded(Scene scene)
    {
        Framework framework = Scenes.FindRootObject<Framework>(scene);
        framework.Init(levelScriptName, loadoutResult, canGoToLevelSelect);
    }

    public void OnOutroComplete()
    {

    }
}
