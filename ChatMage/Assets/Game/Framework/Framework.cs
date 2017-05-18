using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CCC.Manager;
using UnityEngine.Events;

public class Framework : MonoBehaviour
{
    public const string SCENENAME = "Framework";
    public bool loadScenesAsync = false;
    public Game game;
    [Header("Temporaire")]
    public PlayerBuilder playerbuilder;

    [Header("Debug")]
    public LevelScript defaultLevel;

    private LevelScript currentLevel;

    private Vector2 screenBounds;
    private bool hasInit = false;

    private UiSystem uiSystem;
    private Map map;

    [System.NonSerialized]
    private bool mapLoaded = false;
    [System.NonSerialized]
    private bool uiLoaded = false;

    void Start()
    {
        //Debug Init
        //Note: c'est important de sync avec le mastermanager.
        //Sinon, partir de la scène 'Framework' ne marcherait pas
        if (Scenes.SceneCount() == 1 && !hasInit)
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
        hasInit = true;
        currentLevel = level;

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

        //Le UI est déjà loadé, probablement du au mode debug. On ne la reload pas
        if (Scenes.Exists(UiSystem.SCENENAME))
        {
            OnUILoaded(Scenes.GetActive(UiSystem.SCENENAME));
        }
        else
        {
            if (loadScenesAsync)
            {
                Scenes.LoadAsync(UiSystem.SCENENAME, LoadSceneMode.Additive, OnUILoaded);
            }
            else
            {
                Scenes.Load(UiSystem.SCENENAME, LoadSceneMode.Additive, OnUILoaded);
            }
        }
    }

    void OnMapLoaded(Scene scene)
    {
        map = Scenes.FindRootObject<Map>(scene);
        mapLoaded = true;

        if (uiLoaded)
            OnAllLoaded();
    }
    void OnUILoaded(Scene scene)
    {
        uiSystem = Scenes.FindRootObject<UiSystem>(scene);
        uiLoaded = true;

        if (mapLoaded)
            OnAllLoaded();
    }

    void OnAllLoaded()
    {
        //Spawn Character
        Vehicle player = playerbuilder.BuildPlayer();

        //Game Init
        game.Init(currentLevel);

        //Add player to list
        game.AddPlayer(player);

        //Init map
        game.map = map;
        map.Init(game.ScreenBounds.x, game.ScreenBounds.y);

        //Spawner Init
        Game.instance.spawner.Init();

        // UI Init
        uiSystem.Init();

        //Game ready
        game.ReadyGame();
    }
}
