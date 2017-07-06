using CCC.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopUpMenu
{
    public static void ShowOKPopUpMenu(string title, string message, Action onOk = null)
    {
        Scenes.LoadAsync(PopUpScript.SCENENAME, LoadSceneMode.Additive, delegate (Scene scene)
        {
            Scenes.FindRootObject<PopUpScript>(scene)
            .SetOkPopUp(onOk)
            .SetTitle(title)
            .SetMessage(message);
        });
    }

    public static void ShowConfirmPopUpMenu(string title, string message, Action onConfirm, Action onCancel = null)
    {
        Scenes.LoadAsync(PopUpScript.SCENENAME, LoadSceneMode.Additive, delegate (Scene scene)
        {
            Scenes.FindRootObject<PopUpScript>(scene).SetConfirmPopUp(onCancel, onConfirm)
             .SetConfirmPopUp(onCancel, onConfirm)
             .SetTitle(title)
             .SetMessage(message);
        });
    }

    public static void ShowYesNoPopUpMenu(string title, string message, Action onYes, Action onNo = null)
    {
        Scenes.LoadAsync(PopUpScript.SCENENAME, LoadSceneMode.Additive, delegate (Scene scene)
        {
            Scenes.FindRootObject<PopUpScript>(scene)
             .SetYesNoPopUp(onNo, onYes)
             .SetTitle(title)
             .SetMessage(message);
        });
    }
}
