
using CCC.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameOptions : WindowAnimation
{
    public const string SCENENAME = "InGameOptions";

    [Header("Options Menu")]
    public Button levelSelectButton;
    [SerializeField] private AudioMixerSaver audioMixerSaver;

    protected override void Awake()
    {
        base.Awake();

        PersistentLoader.LoadIfNotLoaded(() =>
        {
            if (!IsOpen())
                Open();
        });

        if (Game.Instance != null)
        {
            levelSelectButton.gameObject.SetActive(Game.Instance.framework.CanGoToLevelSelect);

            //Lock game state
            Game.Instance.gameRunning.Lock("optionsMenu");
            Game.Instance.ui.playerInputs.Enabled.Lock("opt");
        }
    }

    protected void OnDestroy()
    {
        if (Application.isPlaying && Game.Instance != null)
        {
            //Unlock game state
            Game.Instance.gameRunning.Unlock("optionsMenu");
            Game.Instance.ui.playerInputs.Enabled.Unlock("opt");
        }
    }

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

    public void RestartGame()
    {
        if (Game.Instance != null)
            Game.Instance.framework.RestartLevel();
    }

    public void BackToLevelSelect()
    {
        LoadingScreen.TransitionTo(LevelSelect.LevelSelection.SCENENAME, null);
    }

    public static void OpenIfClosed()
    {
        if (Game.Instance == null)
        {
            Debug.LogWarning("Cannot open InGameOptions if the game is not running.");
            return;
        }

        if (Scenes.IsActive(SCENENAME))
            return;
        Scenes.LoadAsync(SCENENAME, LoadSceneMode.Additive);
    }
}
