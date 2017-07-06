using CCC.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopPopUpMenu
{
    public static void ShowShopPopUpMenu(string title, string message, Sprite equipableIcon, string price, int amount, Action onConfirm, Action onCancel = null)
    {
        Scenes.LoadAsync(ShopPopUpScript.SCENENAME, LoadSceneMode.Additive, delegate (Scene scene)
        {
            Scenes.FindRootObject<ShopPopUpScript>(scene).SetShopPopUp(onCancel, onConfirm)
             .SetTitle(title)
             .SetMessage(message)
             .SetPrice(price.Replace("-", ""))
             .SetIcon(equipableIcon)
             .SetAmount(amount);
        });
    }
}

