using CCC.Manager;
using CCC.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionWindow : WindowAnimation
{

    public string SCENENAME = "Options";
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
}
