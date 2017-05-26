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
    public InGameEvents events;
    [Header("Temporaire")]
    public PlayerBuilder playerbuilder;

    [Header("Debug")]
    public LevelScript defaultLevel;


    private Vector2 screenBounds;
    private bool hasInit = false;

    private UiSystem uiSystem;
    private Map map;

    private bool mapLoaded = false;
    private bool uiLoaded = false;
    private LevelScript level = null;
    private LoadoutResult loadoutResult;

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
        Init(defaultLevel, null);
    }

    /// <summary>
    /// Start la game avec le level spécifié
    /// </summary>
    public void Init(LevelScript level, LoadoutResult loadoutResult)
    {
        hasInit = true;
        this.level = level;
        this.loadoutResult = loadoutResult;

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
            OnAllScenesLoaded();
    }
    void OnUILoaded(Scene scene)
    {
        uiSystem = Scenes.FindRootObject<UiSystem>(scene);
        uiLoaded = true;

        if (mapLoaded)
            OnAllScenesLoaded();
    }

    void OnAllScenesLoaded()
    {
        InitQueue initQueue = new InitQueue(OnAllModulesLoaded);
        initQueue.canTriggerAction = false;

        //Ajouter dautre module ici si nécessaire
        level.Init(initQueue.Register(), events);
        playerbuilder.LoadAssets(loadoutResult, initQueue.Register());

        if (initQueue.IsDone)
            OnAllModulesLoaded();
        else
            initQueue.canTriggerAction = true;
    }

    void OnAllModulesLoaded()
    {
        //Spawn Character
        PlayerController player = playerbuilder.BuildPlayer();

        //Game Init
        game.map = map;
        game.Init(level,this);

        //Add player to list
        game.AddPlayer(player);

        //Init map
        map.Init(game.ScreenBounds.x, game.ScreenBounds.y);

        //Spawner Init
        Game.instance.spawner.Init();

        // UI Init
        uiSystem.Init(player);
        game.ui = uiSystem;

        //Game ready
        game.ReadyGame();

        LoadingScreen.OnNewSetupComplete();
    }

    public void RestartLevel()
    {
        // TODO FIX THIS SHIT
        LoadingScreen.TransitionTo(SCENENAME,new ToGameMessage(level,loadoutResult),true);
    }
}
