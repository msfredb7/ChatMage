
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
        AudioMixerSaver.Instance.Save();
        Exit();
    }

    public void Cancel()
    {
        AudioMixerSaver.Instance.Load();
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

    public void CHEAT_Invincible()
    {
        Game.Instance.Player.playerStats.damagable = false;
    }
    public void CHEAT_FastSmash()
    {
        SmashManager sm = Game.Instance.smashManager;
        sm.IncreaseSmashJuice(sm.MaxJuice);
        //if (sm.CurrentSmashBall != null)
        //    sm.CurrentSmashBall.ForceDeath();
        //else
        //    sm.DecreaseCooldown(sm.RemainingTime);
    }
    public void CHEAT_Win()
    {
        Game.Instance.levelScript.Win();
    }
    public void CHEAT_Lose()
    {
        Game.Instance.levelScript.Lose();
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
