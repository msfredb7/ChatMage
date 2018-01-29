
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CCC.UI;
using UnityEngine.SceneManagement;

public class OptionsButton : MonoBehaviour
{
    public enum Type { Menu = 0, InGame = 1 }
    public Type type;
    public bool canOpenWithEscape = true;

    void Update()
    {
        if (canOpenWithEscape && Input.GetKeyDown(KeyCode.Escape))
        {
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
