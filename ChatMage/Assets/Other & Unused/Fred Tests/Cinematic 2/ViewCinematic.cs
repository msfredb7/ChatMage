﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewCinematic : MonoBehaviour
{
    public SceneInfo returnScene;
    public SceneInfo cinematicScene;

    public void Launch()
    {
        CinematicEnder.onCompletion = new CinematicEnder.OnCompletion(returnScene.SceneName, null, false);
        LoadingScreen.TransitionTo(cinematicScene.SceneName, null, false, Color.black);
    }
}
