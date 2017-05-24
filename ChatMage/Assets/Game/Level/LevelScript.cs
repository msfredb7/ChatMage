using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using UnityEngine.Events;
using CCC.Manager;
using FullSerializer;

public abstract class LevelScript : BaseScriptableObject
{
    public bool hasWin;
    public string sceneName;

    [fsIgnore]
    public UnityEvent onObjectiveComplete = new UnityEvent();
    [fsIgnore]
    public UnityEvent onObjectiveFailed = new UnityEvent();
    public bool IsOver { get { return isOver; } }
    [fsIgnore]
    public bool isOver = false;

    public InGameEvents events;

    // Init Level Script
    public void Init(System.Action onComplete, InGameEvents events)
    {
        isOver = false;
        hasWin = false;
        Game.instance.onGameReady.AddListener(GameReady);
        Game.instance.onGameStarted.AddListener(GameStarted);
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
        Game.instance.Player.playerStats.onDeath.AddListener(End);
        OnGameStarted();
    }

    protected abstract void OnGameStarted();

    // Update Level Script
    public void Update()
    {
        OnUpdate();
    }

    protected abstract void OnUpdate();

    // End Level Script
    public void End()
    {
        events.End();
        OnEnd();
    }

    protected abstract void OnEnd();

    public abstract void ReceiveEvent(string message);
}
