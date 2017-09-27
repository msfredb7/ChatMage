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
    public Transform unitsContainer;
    public PlayerBounds playerBounds;
    public SmashManager smashManager;
    public HealthPackManager healthPackManager;
    public InGameEvents events;
    public CommonVFX commonVfx;
    public AjusteCadre cadre;

    //Dynamic references
    [fsIgnore, NonSerialized]
    public Map map;
    [fsIgnore, NonSerialized]
    public UiSystem ui;
    [fsIgnore, NonSerialized]
    public LevelScript levelScript;
    [fsIgnore, NonSerialized]
    public Framework framework;
    [fsIgnore, NonSerialized]
    public PlayableArea aiArea = new PlayableArea();

    [InspectorDisabled]
    public LinkedList<Unit> units = new LinkedList<Unit>();
    [InspectorDisabled]
    public LinkedList<Unit> attackableUnits = new LinkedList<Unit>();
    [fsIgnore, NonSerialized]
    private LinkedList<AutoDeactivation> autoDeactivated = new LinkedList<AutoDeactivation>();

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

    private bool destroying = false;

    public void Init(LevelScript level, Framework framework, PlayerController player)
    {
        gameRunning.onLockStateChange += GameRunning_onLockStateChange;

        // Time Scale Reset
        Time.timeScale = 1;

        // Framework
        this.framework = framework;

        // Init LevelScript
        levelScript = level;

        AddPlayer(player);

        gameCamera.Init(player.vehicle);
        healthPackManager.Init(player);

        playerBounds.Resize(gameCamera.ScreenSize.x, -gameCamera.distance);
    }

    private void GameRunning_onLockStateChange(bool state)
    {
        if (!destroying)
            Time.timeScale = gameRunning ? 1 : 0;
    }

    public void ReadyGame()
    {
        gameReady = true;
        if (onGameReady != null)
            onGameReady();
        onGameReady = null;
    }

    /// <summary>
    /// Demare la partie
    /// </summary>
    public void StartGame()
    {
        gameStarted = true;
        if (onGameStarted != null)
            onGameStarted();
        onGameStarted = null;
    }

    private void Update()
    {
        if (gameReady)
        {
            // Coutdown 1-2-3 ??
            if (gameStarted)
            {
                levelScript.Update();
            }
        }

        if (gameCamera.MovedSinceLastFrame)
        {
            float cameraHeight = gameCamera.Height;

            LinkedListNode<AutoDeactivation> node = autoDeactivated.First;
            while (node != null)
            {
                AutoDeactivation val = node.Value;
                val.CheckActivation(cameraHeight);
                node = node.Next;
            }
        }
    }

    public void Quit()
    {
        LoadingScreen.TransitionTo(LevelSelect.LevelSelection.SCENENAME, null);
    }

    protected override void OnDestroy()
    {
        destroying = true;
        base.OnDestroy();

        Time.timeScale = 1;

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
    public void AddExistingUnit(Unit unit, bool putInUnitsContainer = true)
    {
        if (putInUnitsContainer)
            unit.transform.SetParent(unitsContainer);
        unit.TimeScale = worldTimeScale;

        units.AddLast(unit);
        unit.stdNode = units.Last;

        //Auto Deactivation ?
        AutoDeactivation autoDeactivation = unit.GetComponent<AutoDeactivation>();
        if (autoDeactivation != null)
        {
            autoDeactivated.AddLast(autoDeactivation);
            autoDeactivation.gameNode = autoDeactivated.Last;
        }

        //Attackable ?
        if (unit is IAttackable)
        {
            attackableUnits.AddLast(unit);
            unit.attackableNode = attackableUnits.Last;
        }

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
        //est-ce qu'on est entrain de quit ?
        if (instance == null)
            return;

        //On l'enleve de la liste
        units.Remove(unit.stdNode);

        //Auto Deactivation ?
        AutoDeactivation autoDeactivation = unit.GetComponent<AutoDeactivation>();
        if (autoDeactivation != null)
            autoDeactivated.Remove(autoDeactivation.gameNode);

        //Attackable ?
        if (unit is IAttackable)
            attackableUnits.Remove(unit.attackableNode);

        if (onUnitDestroyed != null)
            onUnitDestroyed(unit);
    }

    #endregion
}
