using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CCC.UI;
using UnityEngine.SceneManagement;

public class OptionMenu : MonoBehaviour
{
    public string SCENENAME = "Options";
    public string SCENENAMEINGAME = "InGameOptions";

    public void LoadOptionMenu()
    {
        Scenes.LoadAsync(SCENENAME,LoadSceneMode.Additive);
    }

    public void LoadInGameOptionMenu()
    {
        Scenes.LoadAsync(SCENENAMEINGAME, LoadSceneMode.Additive);
        Game.instance.gameRunning.Lock("optionsMenu");
    }
}
