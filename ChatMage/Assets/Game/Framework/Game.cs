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

    public bool gameReady = false;
    public bool gameStarted = false;

    [InspectorDisabled]
    public List<Unit> units = new List<Unit>();
    public Vehicle Player { get { return player; } }
    private Vehicle player;

    public void Init(LevelScript level)
    {
        //Screen bounds
        screenBounds = new Vector2(cam.orthographicSize * cam.aspect * 2, cam.orthographicSize * 2);
        defaultToRealRatio = new Vector2(defaultBounds.x / screenBounds.x, defaultBounds.y / screenBounds.y);

        //Camera adjustment
        CamAdjustment camAdjustment = cam.GetComponent<CamAdjustment>();
        if (camAdjustment != null)
            camAdjustment.Adjust(screenBounds);

        // Init LevelScript
        currentLevel = level;
        level.Init();
    }

    private void Update()
    {
        if (gameReady)
        {
            // Coutdown 1-2-3 ??
            gameStarted = true;
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
        Unit newUnit = Instantiate(prefab.gameObject).GetComponent<Unit>();

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
