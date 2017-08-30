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
    public CanvasGroup hideGroup;

    public Dialog CurrentDialog { get { return currentDialog != null ? currentDialog.dialog : null; } }
    private RuntimeDialog currentDialog;

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
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && CurrentDialog != null && currentDialog.isActive)
            End();

    }

    public void StartDialog(Dialog dialog, Action onComplete = null)
    {
        // On s'assure que le joueur est bien visible
        Game.instance.Player.playerStats.EnableSprite();

        if (CurrentDialog != null)
            throw new Exception("Cannot start a dialog while another is ongoing.");

        if(dialog == null)
            throw new Exception("Tried to start a null dialog.");

        if (dialog.pauseGame)
            Game.instance.gameRunning.Lock("dialog");

        gameObject.SetActive(true);

        currentDialog = new RuntimeDialog(dialog, onComplete);

        hideGroup.blocksRaycasts = false;
        hideGroup.DOFade(0, textBox.openDuration).SetUpdate(true);
        textBox.Open(OnOpenComplete);
    }

    private void OnOpenComplete()
    {
        currentDialog.isActive = true;
        DisplayNextReply();
    }

    private void OnCloseComplete()
    {
        gameObject.SetActive(false);
        hideGroup.blocksRaycasts = true;

        if (currentDialog.dialog.pauseGame)
            Game.instance.gameRunning.Unlock("dialog");

        if (currentDialog.onComplete != null)
            currentDialog.onComplete();

        currentDialog = null;
    }

    public void OnUIClick()
    {
        if (currentDialog == null || !currentDialog.isActive)
            return;

        if (textBox.IsAnimatingText())
            textBox.SpeedUp();
        else
            DisplayNextReply();
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

        hideGroup.DOFade(1, textBox.openDuration).SetUpdate(true);
        characters.HideBoth();
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
