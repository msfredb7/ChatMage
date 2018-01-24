using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat_ClearSave : CheatButton
{
    public override void Execute()
    {
        gameObject.SetActive(false);

        foreach (DataSaver saver in DataSaverBank.Instance.GetDataSavers())
        {
            saver.ClearSave();
        }

        // Permet de ne pas avoir à dire au joueur: "quitte le level select et revient" pour que ca marche
        LoadingScreen.TransitionTo(MainMenu.SCENENAME, null);
    }
}
