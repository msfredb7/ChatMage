
using CCC.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuOptions : WindowAnimation
{
    public const string SCENENAME = "MenuOptions";
    private bool isQuitting = false;

    public void Confirm()
    {
        if (isQuitting)
            return;
        AudioMixerSaver.Instance.Save();
        Exit();
    }

    public void Cancel()
    {
        if (isQuitting)
            return;
        AudioMixerSaver.Instance.Load();
        Exit();
    }

    private void Exit()
    {
        if (isQuitting) return;

        isQuitting = true;

        if (this != null)
        {
            Close(
                delegate ()
                {
                    Scenes.UnloadAsync(SCENENAME);
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

    public static void OpenIfClosed()
    {
        if (Game.Instance != null)
        {
            Debug.LogWarning("Cannot open MenuOptions if the game is running.");
            return;
        }

        if (Scenes.IsActive(SCENENAME))
            return;
        Scenes.LoadAsync(SCENENAME, LoadSceneMode.Additive);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            print("pls");
            Confirm();
        }
    }



    //------------------------------� enlever------------------------------//

    public List<string> levelToUnlock = new List<string>();

    public void ClearSave()
    {
        if (isQuitting)
            return;
        GameSaves.instance.ClearAllSaves();
        // Permet de ne pas avoir à dire au joueur: "quitte le level select et revient" pour que ca marche
        LoadingScreen.TransitionTo(MainMenu.SCENENAME, null);
    }

    public void GainSmashAccess()
    {
        Armory.UnlockAccessToSmash();
    }
    public void GainItemsAccess()
    {
        Armory.UnlockAccessToItems();
    }

    public void UnlockAllLevels()
    {
        foreach (string levelScriptName in levelToUnlock)
        {
            GameSaves.instance.SetBool(GameSaves.Type.Levels, LevelScript.COMPLETED_KEY + levelScriptName, true);
            GameSaves.instance.SaveData(GameSaves.Type.Levels);
            // Est-ce qu'il y a autre chose à ajouter ?
        }
        // Permet de ne pas avoir à dire au joueur: "quitte le level select et revient" pour que ca marche
        LoadingScreen.TransitionTo(MainMenu.SCENENAME, null);
    }
}
