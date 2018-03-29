
using CCC.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuOptions : WindowAnimation
{
    public const string SCENENAME = "MenuOptions";

    [SerializeField] private AudioMixerSaver audioMixerSaver;

    public void Confirm()
    {
        if (IsOpen())
            audioMixerSaver.Save();
        Close();
    }

    public void Cancel()
    {
        if (IsOpen())
            audioMixerSaver.Load();
        Close();
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
}
