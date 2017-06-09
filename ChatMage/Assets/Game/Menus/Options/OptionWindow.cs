using CCC.Manager;
using CCC.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionWindow : WindowAnimation
{

    public string SCENENAME = "Options";
    public string SCENENAMEINGAME = "InGameOptions";
    private bool quit = false;

    public void Annuler()
    {
        SoundManager.Load();
        Exit();
    }

    public void Confirmer()
    {
        SoundManager.Save();
        Exit();
    }

    public void AnnulerInGame()
    {
        SoundManager.Load();
        ExitInGame();
    }

    public void ConfirmerInGame()
    {
        SoundManager.Save();
        ExitInGame();
    }

    public void Restart()
    {
        Game.instance.framework.RestartLevel();
    }

    public void GoToMenu()
    {
        LoadingScreen.TransitionTo(LevelSelect.LevelSelection.SCENENAME, null);
    }

    public void Exit()
    {
        if (quit) return;

        quit = true;

        if (this != null)
        {
            Close(
                delegate ()
                {
                    Scenes.UnloadAsync(SCENENAME);
                    Time.timeScale = 1;
                    quit = false;
                }
            );
        }
        else
        {
            Scenes.UnloadAsync(SCENENAME);
            quit = false;
        }
    }

    public void ExitInGame()
    {
        if (quit) return;

        quit = true;

        if (this != null)
        {
            Close(
                delegate ()
                {
                    Scenes.UnloadAsync(SCENENAMEINGAME);
                    Time.timeScale = 1;
                    quit = false;
                }
            );
        }
        else
        {
            Scenes.UnloadAsync(SCENENAMEINGAME);
            quit = false;
        }
    }
}
