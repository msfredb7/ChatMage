using CCC.Manager;
using CCC.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopUpScript : WindowAnimation
{
    public static string SCENENAME = "PopUp";


    [Header("Popup"), SerializeField]
    private Text title;
    [SerializeField]
    public Text message;
    [SerializeField]
    public Button cancelButton;
    [SerializeField]
    public Text cancelText;
    [SerializeField]
    public Button confirmButton;
    [SerializeField]
    public Text confirmText;
    [SerializeField]
    public Button okButton;
    [SerializeField]
    public Camera cam;

    private void Start()
    {
        if (Camera.main == null)
            cam.gameObject.SetActive(true);
        MasterManager.Sync();
    }

    public PopUpScript SetOkPopUp(Action okCallback = null)
    {
        confirmButton.gameObject.SetActive(false);
        cancelButton.gameObject.SetActive(false);

        okButton.onClick.AddListener(
            delegate ()
            {
                Close(OnClose);
                if (okCallback != null)
                    okCallback();
            });

        return this;
    }

    public PopUpScript SetConfirmPopUp(Action cancelCallback, Action confirmCallback)
    {
        okButton.gameObject.SetActive(false);

        cancelText.text = "CANCEL";
        confirmText.text = "CONFIRM";


        cancelButton.onClick.AddListener(
            delegate ()
            {
                Close(OnClose);
                if (cancelCallback != null)
                    cancelCallback();
            });
        confirmButton.onClick.AddListener(
            delegate ()
            {
                Close(OnClose);
                if (confirmCallback != null)
                    confirmCallback();
            });

        return this;
    }

    public PopUpScript SetYesNoPopUp(Action noCallback, Action yesCallback)
    {
        okButton.gameObject.SetActive(false);

        cancelText.text = "NO";
        confirmText.text = "YES";

        cancelButton.onClick.AddListener(
            delegate ()
            {
                Close(OnClose);
                if (noCallback != null)
                    noCallback();
            });
        confirmButton.onClick.AddListener(
            delegate ()
            {
                Close(OnClose);
                if (yesCallback != null)
                    yesCallback();
            });

        return this;
    }

    public PopUpScript SetMessage(string text)
    {
        message.text = text;
        return this;
    }

    public PopUpScript SetTitle(string text)
    {
        title.text = text;
        return this;
    }

    private void ClearListeners()
    {
        okButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
        confirmButton.onClick.RemoveAllListeners();
    }

    private void OnClose()
    {
        Scenes.UnloadAsync(SCENENAME);
    }
}
