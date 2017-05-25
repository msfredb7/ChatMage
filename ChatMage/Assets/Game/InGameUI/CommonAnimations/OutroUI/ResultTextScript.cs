using CCC.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultTextScript : MonoBehaviour {

    public Text result;
    public Text score;

    bool hasWin;

    LevelScript levelScript;

    public class GameResultMessage : SceneMessage
    {
        public bool hasWin;
        public LevelScript levelScript;

        public GameResultMessage(bool hasWin, LevelScript levelScript)
        {
            this.hasWin = hasWin;
            this.levelScript = levelScript;
        }

        public void OnLoaded(Scene scene)
        {
            LevelSelection levelSelection = Scenes.FindRootObject<LevelSelection>(scene);
            levelSelection.UpdateWorld(levelScript, hasWin);
        }

        public void OnOutroComplete()
        {
            
        }
    }


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
        LoadingScreen.TransitionTo("MenuSelection", null);
        // A utiliser au besoin
        //LoadingScreen.TransitionTo("MenuSelection", new GameResultMessage(hasWin, levelScript));
    }
}
