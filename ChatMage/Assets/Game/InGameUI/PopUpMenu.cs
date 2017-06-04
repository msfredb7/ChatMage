using CCC.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopUpMenu {

    private static string SCENENAME = "PopUp";

	public static void ShowOKPopUpMenu(string text, Action onOK = null)
    {
        bool popUpExist = false;
        Scenes.LoadAsync(SCENENAME,LoadSceneMode.Additive, delegate(Scene scene)
        {
            GameObject[] rootObjects = scene.GetRootGameObjects();
            for (int i = 0; i < rootObjects.Length; i++)
            {
                PopUpScript currentPopUp = rootObjects[i].GetComponent<PopUpScript>();
                if (currentPopUp != null)
                {
                    currentPopUp.ChangeToOKPopUp();
                    currentPopUp.SetText(text);
                    currentPopUp.ok.GetComponent<Button>().onClick.AddListener(delegate ()
                    {
                        currentPopUp.Close(delegate() {
                            Scenes.Unload(SCENENAME);
                            if (onOK != null)
                                onOK.Invoke();
                        });
                    });
                }
            }
        });
        if (!popUpExist)
            Scenes.UnloadAsync(SCENENAME);
    }

    public static void ShowChoicePopUpMenu(string text, Action onCancel = null, Action onConfirm = null)
    {
        bool popUpExist = false;
        Scenes.LoadAsync(SCENENAME, LoadSceneMode.Additive, delegate (Scene scene)
        {
            GameObject[] rootObjects = scene.GetRootGameObjects();
            for (int i = 0; i < rootObjects.Length; i++)
            {
                PopUpScript currentPopUp = rootObjects[i].GetComponent<PopUpScript>();
                if (currentPopUp != null)
                {
                    currentPopUp.ChangeToChoicePopUp();
                    currentPopUp.SetText(text);
                    Button cancelButton = currentPopUp.cancel.GetComponent<Button>();
                    if (cancelButton != null)
                    {
                        cancelButton.onClick.AddListener(delegate ()
                        {
                            popUpExist = true;
                            currentPopUp.Close();
                            if (onCancel != null)
                                onCancel.Invoke();
                        });
                    }
                    Button confirmButton = currentPopUp.confirm.GetComponent<Button>();
                    if (confirmButton != null)
                    {
                        confirmButton.onClick.AddListener(delegate ()
                        {
                            popUpExist = true;
                            currentPopUp.Close(delegate () {
                                Scenes.Unload(SCENENAME);
                                if (onConfirm != null)
                                    onConfirm.Invoke();
                            });
                        });
                    }
                }
            }
        });
        if (!popUpExist)
            Scenes.UnloadAsync(SCENENAME);
    }

    public static void ShowPopUpMenu(string text, float cooldown, Action onOver = null)
    {
        bool popUpExist = false;
        Scenes.LoadAsync(SCENENAME, LoadSceneMode.Additive, delegate (Scene scene)
        {
            GameObject[] rootObjects = scene.GetRootGameObjects();
            for (int i = 0; i < rootObjects.Length; i++)
            {
                PopUpScript currentPopUp = rootObjects[i].GetComponent<PopUpScript>();
                if (currentPopUp != null)
                {
                    currentPopUp.ChangeToPopUp();
                    currentPopUp.SetText(text);
                    DelayManager.LocalCallTo(delegate ()
                    {
                        popUpExist = true;
                        currentPopUp.Close(delegate () {
                            Scenes.Unload(SCENENAME);
                            if (onOver != null)
                                onOver.Invoke();
                        });
                    }, cooldown, currentPopUp);
                }
            }
        });
        if (!popUpExist)
            Scenes.UnloadAsync(SCENENAME);
    }
}
