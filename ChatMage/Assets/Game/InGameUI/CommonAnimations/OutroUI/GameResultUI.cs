using CCC.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameResultUI : MonoBehaviour {

    public Text result;
    public Text score;

    bool hasWin;

    LevelScript levelScript;

    public void UpdateResult(bool win, LevelScript currentLevel)
    {
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
        LoadingScreen.TransitionTo(LevelSelection.SCENENAME, null);
    }
}
