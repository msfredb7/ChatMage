using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FullInspector;
using FullSerializer;
using CCC.Manager;
using System;

public class Game : PublicSingleton<Game>
{
    //Linked references
    [InspectorHeader("References")]
    public Camera cam;
    public Spawner spawner;
    public Transform unitsContainer;
    public GameBounds gameBounds;
    public SmashManager smashManager;

    //Dynamic references
    [fsIgnore]
    public Map map;
    [fsIgnore]
    public UiSystem ui;
    [fsIgnore]
    public LevelScript currentLevel;
    [fsIgnore]
    public Framework framework;

    public float Aspect { get { return cam.aspect; } }
    [InspectorHeader("Settings")]
    public Vector2 defaultBounds;
    private Vector2 screenBounds;
    private Vector2 worldBounds;
    private Vector2 defaultToRealRatio;

    [InspectorDisabled]
    public List<Unit> units = new List<Unit>();

    // NON AFFICH�

    public PlayerController Player { get { return player; } }
    private PlayerController player;

    [fsIgnore]
    public bool gameReady = false;
    [fsIgnore]
    public bool gameStarted = false;
    public event SimpleEvent onGameReady;
    public event SimpleEvent onGameStarted;

    public void Init(LevelScript level, Framework framework)
    {
        //Screen bounds
        screenBounds = new Vector2(cam.orthographicSize * cam.aspect * 2, cam.orthographicSize * 2);
        worldBounds = new Vector2(screenBounds.x, screenBounds.y);
        defaultToRealRatio = new Vector2(defaultBounds.x / screenBounds.x, defaultBounds.y / screenBounds.y);

        // Framework
        this.framework = framework;

        //Camera adjustment
        CamAdjustment camAdjustment = cam.GetComponent<CamAdjustment>();
        if (camAdjustment != null)
            camAdjustment.Adjust(screenBounds);
        gameBounds.Resize(screenBounds);

        // Init LevelScript
        currentLevel = level;
        level.onObjectiveComplete.AddListener(OnObjectiveComplete);
        level.onObjectiveFailed.AddListener(OnObjectiveFailed);
    }

    public void ReadyGame()
    {
        gameReady = true;
        if (onGameReady != null)
            onGameReady();
    }

    /// <summary>
    /// Demare la partie
    /// </summary>
    public void StartGame()
    {
        gameStarted = true;
        if (onGameStarted != null)
            onGameStarted();
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
        LoadingScreen.TransitionTo(LevelSelection.SCENENAME, null);
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

    public Vector2 ScreenBounds { get { return screenBounds; } }
    public Vector2 WorldBounds { get { return worldBounds; } }

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
        Unit newUnit = Instantiate(prefab.gameObject, position, Quaternion.identity).GetComponent<Unit>();
        AddExistingUnit(newUnit);

        return newUnit;
    }

    /// <summary>
    /// Ajoute une unit existante
    /// </summary>
    public void AddExistingUnit(Unit unit)
    {
        unit.transform.SetParent(unitsContainer);
        unit.movingPlatform = map.rubanPlayer;

        units.Add(unit);

        //Ajoute les listeners
        unit.onDestroy += OnUnitDestroy;
    }

    public void AddPlayer(PlayerController player)
    {
        AddExistingUnit(player.vehicle);
        this.player = player;
    }

    private void OnUnitDestroy(Unit unit)
    {
        units.Remove(unit);
    }

    #endregion
}
