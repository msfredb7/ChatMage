
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CCC.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsButton : MonoBehaviour
{
    public enum Type { Menu = 0, InGame = 1 }
    public Type type;
    public bool canOpenWithEscape = true;

    void Update()
    {
        if (canOpenWithEscape && Input.GetKeyDown(KeyCode.Escape) && WindowController.windowsInFocus.Count == 0)
        {
            var clickAnim = GetComponent<ClickAnimation>();
            if (clickAnim != null)
                clickAnim.ManualClickAnim();

            var buttonSound = GetComponent<ButtonSoundADV>();
            if (buttonSound)
                buttonSound.ManualClick();

            var button = GetComponent<Button>();
            if (button)
                button.onClick.Invoke();
            else
                OpenOptions();
        }
    }

    public void OpenOptions()
    {
        switch (type)
        {
            case Type.Menu:
                MenuOptions.OpenIfClosed();
                break;
            case Type.InGame:
                InGameOptions.OpenIfClosed();
                break;
        }
    }
}
