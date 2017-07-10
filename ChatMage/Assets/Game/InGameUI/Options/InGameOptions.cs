using CCC.Manager;
using CCC.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameOptions : WindowAnimation
{
    public const string SCENENAME = "InGameOptions";
    private bool isQuitting = false;

    public void Confirm()
    {
        SoundManager.Save();
        Exit();
    }

    public void Cancel()
    {
        SoundManager.Load();
        Exit();
    }

    public void RestartGame()
    {
        if (Game.instance != null)
            Game.instance.framework.RestartLevel();
    }

    public void BackToLevelSelect()
    {
        LoadingScreen.TransitionTo(LevelSelect.LevelSelection.SCENENAME, null);
    }

    public void Exit()
    {
        if (isQuitting) return;

        isQuitting = true;

        if (this != null)
        {
            Close(
                delegate ()
                {
                    Scenes.UnloadAsync(SCENENAME);
                    Game.instance.gameRunning.Unlock("optionsMenu");
                    isQuitting = false;
                }
            );
        }
        else
        {
            Scenes.UnloadAsync(SCENENAME);
            isQuitting = false;
        }
    }

    public static void Open()
    {
        if (Game.instance == null)
        {
            Debug.LogWarning("Cannot open InGameOptions if the game is not running.");
            return;
        }

        Scenes.LoadAsync(SCENENAME, LoadSceneMode.Additive);
        Game.instance.gameRunning.Lock("optionsMenu");
    }
}
