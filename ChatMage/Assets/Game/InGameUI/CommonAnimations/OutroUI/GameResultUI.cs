using CCC.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameResultUI : WrapAnimation {

    public Text result;
    public Text score;

    bool hasWin;

    LevelScript levelScript;

    public void Init(bool win, LevelScript currentLevel)
    {
        base.Init();

        if(win)
            result.text = "YOU WIN";
        else
            result.text = "YOU LOST";

        hasWin = win;
        levelScript = currentLevel;
    }

    public void Restart()
    {
        Game.instance.framework.RestartLevel();
    }

    public void GoToMenu()
    {
        LoadingScreen.TransitionTo(LevelSelect.LevelSelection.SCENENAME, null);
    }
}
