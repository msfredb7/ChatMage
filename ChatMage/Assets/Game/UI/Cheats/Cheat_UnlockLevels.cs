using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat_UnlockLevels : CheatButton
{
    public string[] levelsToUnlock = new string[0];
    public DataSaver levelSaver;

    public override void Execute()
    {
        foreach (string levelScriptName in levelsToUnlock)
        {
            levelSaver.SetBool(LevelScript.COMPLETED_KEY + levelScriptName, true);
        }
        levelSaver.Save();

        // Permet de ne pas avoir à dire au joueur: "quitte le level select et revient" pour que ca marche
        LoadingScreen.TransitionTo(MainMenu.SCENENAME, null);
    }
}
