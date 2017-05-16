using System.Collections;
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
        if (Scenes.SceneCount() == 1)
            MasterManager.Sync(Init);
    }

    /// <summary>
    /// Va loader le default level
    /// </summary>
    void Init()
    {
        Init(defaultLevel);
    }

    /// <summary>
    /// Start la game avec le level spécifié
    /// </summary>
    void Init(LevelScript level)
    {
        isLoadingMap = true;
        if (loadScenesAsync)
        {
            Scenes.LoadAsync(level.sceneName, LoadSceneMode.Additive, OnMapLoaded);
        }
        else
        {
            Scenes.Load(level.sceneName, LoadSceneMode.Additive, OnMapLoaded);
        }
    }

    void OnMapLoaded(Scene scene)
    {
        isLoadingMap = false;

        game.Init();
    }
}
