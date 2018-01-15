using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialoguing;
using Dialoguing.Effect;
using System;
using UnityEngine.UI;
using DG.Tweening;

public class DialogDisplay : MonoBehaviour
{
    public TextBox textBox;
    public Characters characters;
    public GameObject dialogContainer;

    public event SimpleEvent onStartDialog;
    public event SimpleEvent onClosingDialog;
    public event SimpleEvent onEndDialog;


    public Dialog CurrentDialog { get { return currentDialog != null ? currentDialog.dialog : null; } }

    private RuntimeDialog currentDialog;
    private bool hpWasShown;
    private float skipTimer;
    private bool tryingToSkip;
    private bool savePermSkipListOnWin = false;

    private class RuntimeDialog
    {
        public Dialog dialog;
        public int nextReply = 0;
        public bool isActive = false;
        public Action onComplete;
        public RuntimeDialog(Dialog dialog, Action onComplete) { this.dialog = dialog; this.onComplete = onComplete; }
    }

    void Awake()
    {
        dialogContainer.SetActive(false);
    }

    public void Init()
    {
        if (!Game.Instance.framework.isARetry)
        {
            DialogSkip.ClearTemporarySkipList();
        }
        Game.Instance.levelScript.onWin += LevelScript_onWin;
    }

    private void LevelScript_onWin()
    {
        if (savePermSkipListOnWin)
            DialogSkip.SavePermanentSkipListAsync();
    }

    void Update()
    {
        if (CurrentDialog != null && currentDialog.isActive)
        {
            //Si on appuie sur une touche (sauf ESCAPE), on speedup
            if (!Application.isMobilePlatform && Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Escape))
            {
                BeginSkipAttempt();
                SpeedUpOrNext();    //Pour speedup
            }

            //Si le joueur ne tiens plus de touche, on cancel le skip
            if (!Input.anyKey)
            {
                EndSkipAttempt();
            }

            //On essaie de skip
            if (tryingToSkip && skipTimer > 0)
            {
                skipTimer -= Time.unscaledDeltaTime;
                if (skipTimer <= 0)
                    End();
            }
        }
    }

    public void BeginSkipAttempt()
    {
        if (CurrentDialog != null && currentDialog.isActive)
        {
            tryingToSkip = true;
            skipTimer = 0.8f;
        }
    }

    public void EndSkipAttempt()
    {
        tryingToSkip = false;
    }

    public void SpeedUpOrNext_Mobile()
    {
        if (Application.isMobilePlatform)
            SpeedUpOrNext();
    }

    public void SpeedUpOrNext()
    {
        if (currentDialog == null || !currentDialog.isActive)
            return;

        if (textBox.IsAnimatingText())
            textBox.SpeedUp();
        else
            DisplayNextReply();
    }

    public void StartDialog(Dialog dialog, Action onComplete = null)
    {

        if (CurrentDialog != null)
            throw new Exception("Cannot start a dialog while another is ongoing.");

        if (dialog == null)
            throw new Exception("Tried to start a null dialog.");

        if (CheckSkipFlags(dialog))
        {
            if (onComplete != null)
                onComplete();
            return;
        }





        // On s'assure que le joueur est bien visible
        Game.Instance.Player.playerStats.EnableSprite();

        if (dialog.pauseGame)
            Game.Instance.gameRunning.Lock("dialog");

        if (onStartDialog != null)
            onStartDialog();

        dialogContainer.SetActive(true);

        currentDialog = new RuntimeDialog(dialog, onComplete);

        GameUI ui = Game.Instance.ui;
        hpWasShown = ui.healthDisplay.IsShown;
        ui.healthDisplay.Hide();

        textBox.Open(OnOpenComplete);
    }

    private bool CheckSkipFlags(Dialog dialog)
    {
        //Est-ce que le dialog a le flag SkipIfLevelCompleted ?
        if ((dialog.skipFlags & SkipFlags.SkipIfLevelCompleted) != 0)
        {
            bool isInList = DialogSkip.IsInPermanentSkip(dialog);
            if (LevelScript.HasBeenCompleted(Game.Instance.levelScript) && isInList)
            {
                return true;
            }
            if (!isInList)
            {
                DialogSkip.AddToPermanentSkipList(dialog);
                savePermSkipListOnWin = true;
            }
        }

        //Est-ce que le dialog a le flag SkipIfRetry ?
        if ((dialog.skipFlags & SkipFlags.SkipIfRetry) != 0)
        {
            if (Game.Instance.framework.isARetry && DialogSkip.IsInTemporarySkip(dialog))
            {
                return true;
            }
            DialogSkip.AddToTemporarySkipList(dialog);
        }

        //Est-ce que le dialog a le flag AlwaysSkip ?
        if ((dialog.skipFlags & SkipFlags.AlwaysSkip) != 0)
        {
            return true;
        }

        return false;
    }

    private void OnOpenComplete()
    {
        currentDialog.isActive = true;
        DisplayNextReply();
    }

    private void OnCloseComplete()
    {
        dialogContainer.SetActive(false);

        if (currentDialog.dialog.pauseGame)
            Game.Instance.gameRunning.Unlock("dialog");

        if (currentDialog.onComplete != null)
            currentDialog.onComplete();

        if (onEndDialog != null)
            onEndDialog();

        currentDialog = null;
    }

    private void DisplayNextReply()
    {
        Reply[] replies = currentDialog.dialog.replies;
        int index = currentDialog.nextReply;

        if (index >= replies.Length)
        {
            End();
        }
        else
        {
            //Next reply !
            DisplayReply(replies[index]);
            currentDialog.nextReply++;
        }
    }

    private void End()
    {
        //On a fini
        currentDialog.isActive = false;
        tryingToSkip = false;


        GameUI ui = Game.Instance.ui;
        if (hpWasShown)
            ui.healthDisplay.Show();
        //if (smashWasShown)
        //    ui.smashDisplay.Show(true);

        if (onClosingDialog != null)
            onClosingDialog();


        characters.HideBoth();
        characters.DisableLeftName();
        characters.DisableRightName();
        textBox.Close(OnCloseComplete);
    }

    private void DisplayReply(Reply reply)
    {
        //Text
        textBox.DisplayMessage(reply.message);

        //Apply effects
        for (int i = 0; i < reply.effects.Length; i++)
        {
            reply.effects[i].Apply(this, reply);
        }
    }
}
