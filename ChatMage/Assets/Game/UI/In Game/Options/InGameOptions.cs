
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

    private bool isQuitting = false;

    protected override void Awake()
    {
        base.Awake();

        PersistentLoader.LoadIfNotLoaded(()=>
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

    private void OnDestroy()
    {
        if (Application.isPlaying && isQuitting && Game.Instance != null)
        {
            //Unlock game state
            Game.Instance.gameRunning.Unlock("optionsMenu");
            Game.Instance.ui.playerInputs.Enabled.Unlock("opt");
        }
    }

    public void Confirm()
    {
        audioMixerSaver.Save();
        Exit();
    }

    public void Cancel()
    {
        audioMixerSaver.Load();
        Exit();
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

    public void Exit()
    {
        if (isQuitting) return;

        isQuitting = true;

        if (this != null)
        {
            Close(
                delegate ()
                {
                    Scenes.UnloadAsync(SCENENAME);
                }
            );
        }
        else
        {
            Scenes.UnloadAsync(SCENENAME);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Confirm();
        }
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
