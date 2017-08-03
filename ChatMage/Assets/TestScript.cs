using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using CCC.Manager;
using CCC.Utility;
using UnityEngine.UI;
using FullInspector;

public class TestScript : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            //AdditiveCinematicSettings settings = new AdditiveCinematicSettings();
            //settings.skipOnDoubleTap = true;
            //AdditiveCinematicScene.LaunchCinematic("Additive Cinematic Demo", settings);
            CinematicSettings settings = new CinematicSettings();
            settings.skipOnDoubleTap = true;
            settings.nextSceneName = LevelSelect.LevelSelection.SCENENAME;
            settings.nextSceneMessage = null;
            CinematicScene.LaunchCinematic("Cinematic Demo", settings);
        }
    }
}