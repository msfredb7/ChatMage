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
        SetTimeScale(0);
    }

    public void LoadInGameOptionMenu()
    {
        Scenes.LoadAsync(SCENENAMEINGAME, LoadSceneMode.Additive);
        SetTimeScale(0);
    }

    void SetTimeScale(float amount)
    {
        List<Unit> units = Game.instance.units;
        for (int i = 0; i < units.Count; i++)
        {
            units[i].TimeScale = amount;
        }
        if (amount == 1)
            Game.instance.worldTimeScale.RemoveBuff("zwrdo");
        else
            Game.instance.worldTimeScale.AddBuff("zwrdo", amount * 100 - 100, CCC.Utility.BuffType.Percent);
    }
}
