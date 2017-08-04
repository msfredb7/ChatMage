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


    private Vector2 screenBounds;
    private bool hasInit = false;

    private UiSystem uiSystem;
    private Map map;

    private bool mapLoaded = false;
    private bool uiLoaded = false;
    private LevelScript level = null;
    private LoadoutResult loadoutResult;
    private bool canGoToLevelSelect;

    public bool CanGoToLevelSelect { get { return canGoToLevelSelect; } }

    void Start()
    {
        //Debug Init
        //Note: c'est important de sync avec le mastermanager.
        //Sinon, partir de la scène 'Framework' ne marcherait pas
        if (Scenes.SceneCount() == 1 && !hasInit)
            MasterManager.Sync(Init);
    }

    //LE init utilisé pour start tout
    public void Init(string levelScriptName, LoadoutResult loadoutResult, bool canGoToLevelSelect = true)
    {
        ResourceLoader.LoadLevelScriptAsync(levelScriptName, delegate (LevelScript levelScript)
        {
            Init(levelScript, loadoutResult, canGoToLevelSelect);
        });
    }

    /// <summary>
    /// Va loader le default level
    /// </summary>
    private void Init()
    {
        Init(defaultLevel, null, true);
    }

    /// <summary>
    /// Start la game avec le level spécifié
    /// </summary>
    private void Init(LevelScript level, LoadoutResult loadoutResult, bool canGoToLevelSelect)
    {
        hasInit = true;
        this.level = level;
        this.loadoutResult = loadoutResult;
        this.canGoToLevelSelect = canGoToLevelSelect;

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

        //Ajouter dautre module ici si nécessaire
        level.Init(initQueue.Register());
        playerbuilder.LoadAssets(loadoutResult, initQueue.Register());

        initQueue.MarkEnd();
    }

    void OnAllModulesLoaded()
    {
        //Spawn Character
        PlayerController player = playerbuilder.BuildPlayer();

        //Game Init
        game.map = map;
        game.Init(level,this, player);

        //Init map
        map.Init(player);

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
        LoadingScreen.TransitionTo(SCENENAME,new ToGameMessage(level.name,loadoutResult),true);
    }
}
