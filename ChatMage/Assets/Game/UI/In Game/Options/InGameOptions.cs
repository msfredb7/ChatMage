using CCC.Manager;
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

        levelSelectButton.gameObject.SetActive(Game.instance.framework.CanGoToLevelSelect);
    }

    public void Confirm()
    {
        SoundManager.Save();
        Exit();
    }

    public void Cancel()
    {
        SoundManager.Load();
        Exit();
    }

    public void RestartGame()
    {
        if (Game.instance != null)
            Game.instance.framework.RestartLevel();
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
                    isQuitting = false;
                    OnQuit();
                }
            );
        }
        else
        {
            Scenes.UnloadAsync(SCENENAME);
            isQuitting = false;
            OnQuit();
        }
    }

    public void CHEAT_Invincible()
    {
        Game.instance.Player.playerStats.damagable = false;
    }
    public void CHEAT_FastSmash()
    {
        SmashManager sm = Game.instance.smashManager;
        sm.BoostSmashCounter(sm.smashCounterMax);
        //if (sm.CurrentSmashBall != null)
        //    sm.CurrentSmashBall.ForceDeath();
        //else
        //    sm.DecreaseCooldown(sm.RemainingTime);
    }
    public void CHEAT_Win()
    {
        Game.instance.levelScript.Win();
    }
    public void CHEAT_Lose()
    {
        Game.instance.levelScript.Lose();
    }

    public static void Open()
    {
        if (Game.instance == null)
        {
            Debug.LogWarning("Cannot open InGameOptions if the game is not running.");
            return;
        }

        Scenes.LoadAsync(SCENENAME, LoadSceneMode.Additive);
        OnStartOpen();
    }

    static void OnStartOpen()
    {
        Game.instance.gameRunning.Lock("optionsMenu");
        Game.instance.ui.playerInputs.Enabled.Lock("opt");
    }

    static void OnQuit()
    {
        Game.instance.gameRunning.Unlock("optionsMenu");
        Game.instance.ui.playerInputs.Enabled.Unlock("opt");
    }
}
