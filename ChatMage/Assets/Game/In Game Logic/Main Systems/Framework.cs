using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CCC.Manager;
using UnityEngine.Events;

public class Framework : MonoBehaviour
{
    public const string SCENENAME = "Framework";
    public Game game;

    public bool loadScenesAsync = false;
    public SceneInfo gameUiScene;

    [Header("Temporaire")]
    public PlayerBuilder playerbuilder;

    [Header("Debug")]
    public LevelScript defaultLevel;


    private Vector2 screenBounds;
    private bool hasInit = false;

    private GameUI uiSystem;
    private Map map;

    private bool mapLoaded = false;
    private bool uiLoaded = false;
    private LevelScript levelScript = null;
    private LoadoutResult loadoutResult;
    private bool canGoToLevelSelect;

    [System.NonSerialized]
    public bool isARetry = false;
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
        this.levelScript = level;
        this.loadoutResult = loadoutResult;
        this.canGoToLevelSelect = canGoToLevelSelect;

        //La map est déjà loadé, probablement du au mode debug. On ne la reload pas
        if (level.sceneInfo.IsActive())
        {
            OnMapLoaded(Scenes.GetActive(level.sceneInfo.SceneName));
        }
        else
        {
            if (loadScenesAsync)
            {
                level.sceneInfo.LoadSceneAsync(OnMapLoaded);
            }
            else
            {
                level.sceneInfo.LoadScene(OnMapLoaded);
            }
        }

        //Le UI est déjà loadé, probablement du au mode debug. On ne la reload pas
        if (gameUiScene.IsActive())
        {
            OnUILoaded(Scenes.GetActive(gameUiScene.SceneName));
        }
        else
        {
            if (loadScenesAsync)
            {
                gameUiScene.LoadSceneAsync(OnUILoaded);
            }
            else
            {
                gameUiScene.LoadScene(OnUILoaded);
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
        uiSystem = Scenes.FindRootObject<GameUI>(scene);
        uiLoaded = true;

        if (mapLoaded)
            OnAllScenesLoaded();
    }

    void OnAllScenesLoaded()
    {
        InitQueue initQueue = new InitQueue(OnAllModulesLoaded);

        //Ajouter dautre module ici si nécessaire
        levelScript.Init(initQueue.Register());
        playerbuilder.LoadAssets(loadoutResult, initQueue.Register());

        initQueue.MarkEnd();
    }

    void OnAllModulesLoaded()
    {
        game.map = map;
        game.ui = uiSystem;

        //Spawn Character
        PlayerController player = playerbuilder.BuildPlayer();

        //Game Init
        game.Init(levelScript,this, player);

        //Init map
        map.Init(player);

        // UI Init
        uiSystem.Init(player);

        //Game ready
        game.ReadyGame();

        LoadingScreen.OnNewSetupComplete();
    }

    public void RestartLevel()
    {
        ToGameMessage gameMessage = new ToGameMessage(levelScript.name, loadoutResult);
        gameMessage.SetHasARetry(true);
        LoadingScreen.TransitionTo(SCENENAME, gameMessage, true);
    }
}
