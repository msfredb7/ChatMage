using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class QuickSelector
{
    [MenuItem("The Time Drifter/Select Player %&p", priority = 10)]
    public static void SelectPlayer()
    {
        if(Game.Instance != null && Game.Instance.Player != null)
        {
            Selection.activeGameObject = Game.Instance.Player.gameObject;
        }
    }
    [MenuItem("The Time Drifter/Select Player %&p", true)]
    static bool ValidateSelectPlayer()
    {
        return Game.Instance != null && Game.Instance.Player != null;
    }

    [MenuItem("The Time Drifter/Select Camera %&c", priority = 10)]
    public static void SelectCamera()
    {
        if (Game.Instance != null && Game.Instance.gameCamera != null)
        {
            Selection.activeGameObject = Game.Instance.gameCamera.cam.gameObject;
        }
    }
    [MenuItem("The Time Drifter/Select Camera %&c", true)]
    static bool ValidateSelectCamera()
    {
        return Game.Instance != null && Game.Instance.gameCamera != null;
    }
}
