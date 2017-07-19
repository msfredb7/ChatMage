using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FullInspector;
using FullSerializer;
using CCC.Utility;
using CCC.Manager;
using System;

public class Game : PublicSingleton<Game>
{
    //Linked references
    [InspectorHeader("References")]
    public GameCamera gameCamera;
    public Spawner spawner;
    public Transform unitsContainer;
    public PlayerBounds playerBounds;
    public SmashManager smashManager;
    public HealthPackManager healthPackManager;

    //Dynamic references
    [fsIgnore, NonSerialized]
    public Map map;
    [fsIgnore, NonSerialized]
    public UiSystem ui;
    [fsIgnore, NonSerialized]
    public LevelScript currentLevel;
    [fsIgnore, NonSerialized]
    public Framework framework;
    [fsIgnore, NonSerialized]
    public PlayableArea aiArea = new PlayableArea();

    [InspectorDisabled]
    public List<Unit> units = new List<Unit>();

    // NON AFFICH�

    [fsIgnore]
    public StatFloat worldTimeScale = new StatFloat(1, 0, float.MaxValue, BoundMode.Cap);
    public PlayerController Player { get { return player; } }
    public bool IsPlayerVisible { get { return player != null && player.gameObject.activeSelf && player.playerStats.isVisible; } }
    private PlayerController player;

    [fsIgnore, NonSerialized]
    public Locker gameRunning = new Locker();

    [fsIgnore]
    public bool gameReady = false;
    [fsIgnore]
    public bool gameStarted = false;
    public event SimpleEvent onGameReady;
    public event SimpleEvent onGameStarted;
    public event SimpleEvent onDestroy;
    public event Unit.Unit_Event onUnitSpawned;
    public event Unit.Unit_Event onUnitDestroyed;

    //public bool unitSnap_horizontalBound;
    //public float unitSnap_horizontalBorderWidth;
    //public bool unitSnap_verticalBound;
    //public float unitSnap_verticalBorderWidth;

    public void Init(LevelScript level, Framework framework, PlayerController player)
    {
        gameRunning.onLockStateChange += GameRunning_onLockStateChange;

        // Time Scale Reset
        Time.timeScale = 1;

        // Framework
        this.framework = framework;

        // Init LevelScript
        currentLevel = level;

        AddPlayer(player);

        gameCamera.Init(player.vehicle);
        healthPackManager.Init(player);

        playerBounds.Resize(gameCamera.ScreenSize.x, -gameCamera.distance);
    }

    private void GameRunning_onLockStateChange(bool state)
    {
        Time.timeScale = gameRunning ? 1 : 0;
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
        //OPTIMISATION: Faire une liste a part (une liste de unit pour lequelle il faut 'CheckActivation')
        if (gameCamera.MovedSinceLastFrame)
        {
            for (int i = 0; i < units.Count; i++)
            {
                if (units[i] != player.vehicle)
                    units[i].CheckActivation();
            }
        }
    }

    public void Quit()
    {
        LoadingScreen.TransitionTo(LevelSelect.LevelSelection.SCENENAME, null);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (onDestroy != null)
            onDestroy();
    }

    #region Unit Managment

    /// <summary>
    /// Spawn une unit dans la map
    /// </summary>
    public T SpawnUnit<T>(T prefab, Vector2 position) where T : Unit
    {
        T newUnit = Instantiate(prefab.gameObject, position, Quaternion.identity).GetComponent<T>();
        AddExistingUnit(newUnit);

        return newUnit;
    }

    /// <summary>
    /// Ajoute une unit existante
    /// </summary>
    public void AddExistingUnit(Unit unit)
    {
        unit.transform.SetParent(unitsContainer);
        unit.TimeScale = worldTimeScale;

        units.Add(unit);

        if (onUnitSpawned != null)
            onUnitSpawned(unit);

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

        //est-ce qu'on est entrain de quit ?
        if (instance == null)
            return;

        if (onUnitDestroyed != null)
            onUnitDestroyed(unit);
    }

    //public void SetUnitSnapBorders(bool horizontalBound, float horizontalBorderWidth, bool verticalBound, float verticalBorderWidth)
    //{
    //    unitSnap_horizontalBound = horizontalBound;
    //    unitSnap_horizontalBorderWidth = horizontalBorderWidth;
    //    unitSnap_verticalBound = verticalBound;
    //    unitSnap_verticalBorderWidth = verticalBorderWidth;
    //}

    #endregion
}
