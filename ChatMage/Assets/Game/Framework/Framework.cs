using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CCC.Manager;

public class Framework : MonoBehaviour
{
    public bool loadScenesAsync = false;
    public Camera cam;
    public float Aspect { get { return cam.aspect; } }
    public Vector2 ScreenBounds { get { return screenBounds; } }

    [Header("Debug")]
    public LevelScript defaultLevel;

    private bool isLoadingMap;
    private Vector2 screenBounds;


    void Start()
    {
        screenBounds = new Vector2(cam.orthographicSize * cam.aspect * 2, cam.orthographicSize * 2);

        if (Scenes.SceneCount() == 1)
            Init();
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

    }
}
