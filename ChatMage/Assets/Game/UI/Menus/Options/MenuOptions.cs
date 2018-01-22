
using CCC.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuOptions : WindowAnimation
{
    public const string SCENENAME = "MenuOptions";
    private bool isQuitting = false;

    [Header("Cheats"), SerializeField] private List<DataSaver> saves = new List<DataSaver>();
    [SerializeField] private DataSaver levelsSaver;
    [SerializeField] private DataSaver armorySaver;
    [SerializeField] private List<string> levelsToUnlock = new List<string>();

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



    //------------------------------CHEATS------------------------------//

    public void ClearSave()
    {
        if (isQuitting)
            return;

        foreach (DataSaver saver in saves)
        {
            saver.ClearSave();
        }

        // Permet de ne pas avoir à dire au joueur: "quitte le level select et revient" pour que ca marche
        LoadingScreen.TransitionTo(MainMenu.SCENENAME, null);
    }

    public void GainSmashAccess()
    {
        Armory.UnlockAccessToSmash(armorySaver);
    }
    public void GainItemsAccess()
    {
        Armory.UnlockAccessToItems(armorySaver);
    }

    public void UnlockAllLevels()
    {
        foreach (string levelScriptName in levelsToUnlock)
        {
            levelsSaver.SetBool(LevelScript.COMPLETED_KEY + levelScriptName, true);
        }
        levelsSaver.Save();

        // Permet de ne pas avoir à dire au joueur: "quitte le level select et revient" pour que ca marche
        LoadingScreen.TransitionTo(MainMenu.SCENENAME, null);
    }
}
