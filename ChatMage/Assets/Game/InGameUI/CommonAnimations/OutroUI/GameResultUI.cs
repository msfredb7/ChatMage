using CCC.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameResultUI : BaseOutro {

    public Text result;
    public Text score;

    public void Restart()
    {
        Game.instance.framework.RestartLevel();
    }

    public void GoToMenu()
    {
        LoadingScreen.TransitionTo(LevelSelect.LevelSelection.SCENENAME, null);
    }

    public override void Play(bool hasWon)
    {
        if (hasWon)
            result.text = "YOU WIN";
        else
            result.text = "YOU LOST";
    }
}
