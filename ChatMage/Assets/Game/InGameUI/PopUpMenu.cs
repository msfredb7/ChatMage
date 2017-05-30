using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopUpMenu {

    private static string SCENENAME = "PopUp";

	public static void ShowOKPopUpMenu(string text)
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
                        currentPopUp.Close();
                    });
                }
            }
        });
        if (!popUpExist)
            Scenes.UnloadAsync(SCENENAME);
    }

    public static bool ShowChoicePopUpMenu(string text)
    {
        bool popUpExist = false;
        bool result = false;
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
                            result = false;
                            popUpExist = true;
                            currentPopUp.Close();
                        });
                    }
                    Button confirmButton = currentPopUp.confirm.GetComponent<Button>();
                    if (confirmButton != null)
                    {
                        confirmButton.onClick.AddListener(delegate ()
                        {
                            result = true;
                            popUpExist = true;
                            currentPopUp.Close();
                        });
                    }
                }
            }
        });
        if (!popUpExist)
        {
            Scenes.UnloadAsync(SCENENAME);
            return false;
        }
        else
            return result;
    }

    public static void ShowPopUpMenu(string text, float cooldown)
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
                        currentPopUp.Close();
                        popUpExist = true;
                    }, cooldown, currentPopUp);
                }
            }
        });
        if (!popUpExist)
            Scenes.UnloadAsync(SCENENAME);
    }
}
