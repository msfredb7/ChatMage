using CCC.Manager;
using CCC.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopPopUpScript : WindowAnimation
{
    public static string SCENENAME = "ShopPopUp";


    [Header("ShopPopup"), SerializeField]
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
    public Camera cam;
    [SerializeField]
    public Image icon;
    [SerializeField]
    public Text price;
    [SerializeField]
    public Text amount;

    private void Start()
    {
        if (Camera.main == null)
            cam.gameObject.SetActive(true);
        MasterManager.Sync();
    }

    public ShopPopUpScript SetShopPopUp(Action cancelCallback, Action confirmCallback)
    {
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

    public ShopPopUpScript SetMessage(string text)
    {
        message.text = text;
        return this;
    }

    public ShopPopUpScript SetTitle(string text)
    {
        title.text = text;
        return this;
    }

    public ShopPopUpScript SetPrice(string price)
    {
        this.price.text = "for " + price;
        return this;
    }

    public ShopPopUpScript SetIcon(Sprite icon)
    {
        this.icon.sprite = icon;
        return this;
    }

    public ShopPopUpScript SetAmount(int amount)
    {
        this.amount.text = "X " + amount;
        return this;
    }

    private void ClearListeners()
    {
        cancelButton.onClick.RemoveAllListeners();
        confirmButton.onClick.RemoveAllListeners();
    }

    private void OnClose()
    {
        Scenes.UnloadAsync(SCENENAME);
    }
}
