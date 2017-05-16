﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CCC.Manager;
using UnityEngine.Events;

public class Framework : MonoBehaviour
{
    public bool loadScenesAsync = false;
    public Game game;

    [Header("Debug")]
    public LevelScript defaultLevel;

    private bool isLoadingMap;
    private Vector2 screenBounds;


    void Start()
    {
        //Debug Init
        //Note: c'est important de sync avec le mastermanager.
        //Sinon, partir de la scène 'Framework' ne marcherait pas
        if (Scenes.SceneCount() == 1)
            MasterManager.Sync(Init);
    }

    /// <summary>
    /// Va loader le default level
    /// </summary>
    public void Init()
    {
        Init(defaultLevel);
    }

    /// <summary>
    /// Start la game avec le level spécifié
    /// </summary>
    public void Init(LevelScript level)
    {
        print("Test");
        isLoadingMap = true;

        //La map est déjà loadé, probablement du au mode debug. On ne la reload pas
        if (Scenes.Exists(level.sceneName))
        {
            OnMapLoaded(Scenes.GetActive(level.sceneName));
        }
        else
        {
            if (loadScenesAsync)
            {
                Scenes.LoadAsync(level.sceneName, LoadSceneMode.Additive, OnMapLoaded);
            }
            else
            {
                Scenes.Load(level.sceneName, LoadSceneMode.Additive, OnMapLoaded);
            }
        }
    }

    void OnMapLoaded(Scene scene)
    {
        isLoadingMap = false;

        game.Init();
    }
}
