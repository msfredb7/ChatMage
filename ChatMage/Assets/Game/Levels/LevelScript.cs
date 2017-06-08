using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using UnityEngine.Events;
using CCC.Manager;
using FullSerializer;

public abstract class LevelScript : BaseScriptableObject
{
    public const string WINRESULT_KEY = "winr";
    [fsIgnore]
    public bool hasWon = false;
    public string sceneName;

    public class NextLevel
    {
        public int regionNumber;
        public int levelNumber;
    }

    [fsIgnore]
    public UnityEvent onObjectiveComplete = new UnityEvent();
    [fsIgnore]
    public UnityEvent onObjectiveFailed = new UnityEvent();


    public bool IsOver { get { return isOver; } }
    [fsIgnore]
    public bool isOver = false;

    [fsIgnore]
    public InGameEvents events;

    // Init Level Script
    public void Init(System.Action onComplete, InGameEvents events)
    {
        isOver = false;
        hasWon = false;
        Game.instance.onGameReady += GameReady;
        Game.instance.onGameStarted += GameStarted;
        this.events = events;
        events.Init(this);
        OnInit(onComplete);
    }

    public abstract void OnInit(System.Action onComplete);

    // Game Ready for Level Script
    public void GameReady()
    {
        OnGameReady();
    }

    protected abstract void OnGameReady();

    // Game Started for Level Script
    public void GameStarted()
    {
        Game.instance.Player.vehicle.onDeath += End;
        OnGameStarted();
    }

    protected abstract void OnGameStarted();

    // Update Level Script
    public void Update()
    {
        OnUpdate();
    }

    protected abstract void OnUpdate();

    // End Level Script. WE DO NOT QUIT YET
    public void End(Unit player = null)
    {
        GameSaves.instance.SetBool(GameSaves.Type.LevelSelect, WINRESULT_KEY, hasWon);
        GameSaves.instance.SaveData(GameSaves.Type.LevelSelect);
        OnEnd();
    }

    public abstract void OnEnd();

    public abstract void ReceiveEvent(string message);
}
