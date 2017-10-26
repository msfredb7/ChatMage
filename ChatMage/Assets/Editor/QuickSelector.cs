using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class QuickSelector
{
    [MenuItem("The Time Drifter/Select Player %&p", priority = 10)]
    public static void SelectPlayer()
    {
        if(Game.instance != null && Game.instance.Player != null)
        {
            Selection.activeGameObject = Game.instance.Player.gameObject;
        }
    }
    [MenuItem("The Time Drifter/Select Player %&p", true)]
    static bool ValidateSelectPlayer()
    {
        return Game.instance != null && Game.instance.Player != null;
    }

    [MenuItem("The Time Drifter/Select Camera %&c", priority = 10)]
    public static void SelectCamera()
    {
        if (Game.instance != null && Game.instance.gameCamera != null)
        {
            Selection.activeGameObject = Game.instance.gameCamera.cam.gameObject;
        }
    }
    [MenuItem("The Time Drifter/Select Camera %&c", true)]
    static bool ValidateSelectCamera()
    {
        return Game.instance != null && Game.instance.gameCamera != null;
    }
}
