using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FullInspector;
using FullSerializer;
using CCC.Manager;

public class Game : PublicSingleton<Game>
{
    //Linked references
    [InspectorHeader("References")]
    public Camera cam;
    public PlayerInput playerInput;
    public Spawner spawner;

    //Dynamic references
    [fsIgnore]
    public Map map;

    private LevelScript currentLevel;

    public float Aspect { get { return cam.aspect; } }
    public Vector2 ScreenBounds { get { return screenBounds; } }
    [InspectorHeader("Settings")]
    public Vector2 defaultBounds;
    private Vector2 screenBounds;
    private Vector2 defaultToRealRatio;

    [InspectorDisabled]
    public List<Unit> units = new List<Unit>();

    // NON AFFICHÉ

    public Vehicle Player { get { return player; } }
    private Vehicle player;
    
    [fsIgnore]
    public bool gameReady = false;
    [fsIgnore]
    public bool gameStarted = false;
    [fsIgnore]
    public UnityEvent onGameReady = new UnityEvent();
    [fsIgnore]
    public UnityEvent onGameStarted = new UnityEvent();

    public void Init(LevelScript level)
    {
        LevelScript levelClone = Object.Instantiate(level) as LevelScript;
        
        //Screen bounds
        screenBounds = new Vector2(cam.orthographicSize * cam.aspect * 2, cam.orthographicSize * 2);
        defaultToRealRatio = new Vector2(defaultBounds.x / screenBounds.x, defaultBounds.y / screenBounds.y);

        //Camera adjustment
        CamAdjustment camAdjustment = cam.GetComponent<CamAdjustment>();
        if (camAdjustment != null)
            camAdjustment.Adjust(screenBounds);

        // Init LevelScript
        currentLevel = level;
        level.onObjectiveComplete.AddListener(OnObjectiveComplete);
        level.onObjectiveFailed.AddListener(OnObjectiveFailed);
        level.Init();
    }

    public void ReadyGame()
    {
        gameReady = true;
        onGameReady.Invoke();

        // countdown ?
        StartGame();
    }

    /// <summary>
    /// Demare la partie
    /// </summary>
    public void StartGame()
    {
        gameStarted = true;
        onGameStarted.Invoke();
    }

    private void Update()
    {
        if (gameReady)
        {
            // Coutdown 1-2-3 ??
            if (gameStarted)
            {
                currentLevel.Update();
            }
        }
    }

    public void Quit()
    {
        Scenes.Load("MenuSelection");
    }


    private void OnObjectiveComplete()
    {
        //Win screen !
        Quit();
    }
    private void OnObjectiveFailed()
    {
        // Lose screen !
        Quit();
    }

    #region Bounds

    public Vector3 ConvertToRealPos(Vector3 position)
    {
        return new Vector3(position.x / defaultToRealRatio.x, position.y / defaultToRealRatio.y, 0);
    }
    public Vector2 ConvertToRealPos(Vector2 position)
    {
        return new Vector2(position.x / defaultToRealRatio.x, position.y / defaultToRealRatio.y);
    }

    #endregion

    #region Unit Managment

    /// <summary>
    /// Spawn une unit dans la map
    /// </summary>
    public Unit SpawnUnit(Unit prefab, Vector2 position)
    {
        Unit newUnit = Instantiate(prefab.gameObject,position, Quaternion.identity).GetComponent<Unit>();

        AddExistingUnit(newUnit);

        return newUnit;
    }

    /// <summary>
    /// Ajoute une unit existante
    /// </summary>
    public void AddExistingUnit(Unit unit)
    {
        units.Add(unit);

        //Ajoute les listeners
        unit.onDestroy.AddListener(OnUnitDestroy);
    }

    public void AddPlayer(Vehicle player)
    {
        AddExistingUnit(player);
        this.player = player;
        playerInput.Init(player.GetComponent<PlayerController>());
    }

    private void OnUnitDestroy(Unit unit)
    {
        units.Remove(unit);
    }

    #endregion
}
