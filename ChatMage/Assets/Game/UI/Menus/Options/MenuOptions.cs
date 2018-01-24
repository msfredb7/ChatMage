
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
            Confirm();
        }
    }
}
